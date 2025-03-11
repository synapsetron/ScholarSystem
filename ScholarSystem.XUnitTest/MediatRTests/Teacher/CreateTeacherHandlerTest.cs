using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Application.MediatR.Teachers.Create;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;

namespace ScholarSystem.UnitTests.MediatR.Teachers
{
    public class CreateTeacherHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<CreateTeacherHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly CreateTeacherHandler _handler;

        public CreateTeacherHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<CreateTeacherHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Teacher, CreateTeacherDTO>().ReverseMap());
            _mapper = config.CreateMapper();

            _handler = new CreateTeacherHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTeacher_WhenEmailIsUnique()
        {
            // Arrange
            var request = new CreateTeacherCommand(GetCreateTeacherDTO());
            var teacher = _mapper.Map<Teacher>(request.teacherDTO);

            SetupTeacherDoesNotExist(request.teacherDTO.Email);
            SetupTeacherCreation(teacher);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, request.teacherDTO.Name);
            VerifyRepositoryCalls();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherWithSameEmailExists()
        {
            // Arrange
            var request = new CreateTeacherCommand(GetCreateTeacherDTO());
            var existingTeacher = new Teacher { Id = 1, Name = "Existing Teacher", Email = request.teacherDTO.Email };

            SetupTeacherExists(existingTeacher);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Teacher with email {request.teacherDTO.Email} already exists.");
            VerifyNoTeacherCreation();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateTeacherCommand(GetCreateTeacherDTO());

            SetupTeacherThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while creating the teacher.");
            VerifyNoTeacherCreation();
            VerifyLogging(LogLevel.Error, 1);
        }

        // 🔹 Helper methods

        private static CreateTeacherDTO GetCreateTeacherDTO() =>
            new() { Name = "New Teacher", Email = "new.teacher@example.com" };

        private void SetupTeacherDoesNotExist(string email)
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync(teacher);
        }

        private void SetupTeacherThrowsException()
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupTeacherCreation(Teacher teacher)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.Create(It.IsAny<Teacher>()))
                .Returns(teacher);
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyRepositoryCalls()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Create(It.IsAny<Teacher>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoTeacherCreation()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Create(It.IsAny<Teacher>()), Times.Never);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        private void VerifyLogging(LogLevel level, int times)
        {
            _mockLogger.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(times));
        }

        private static void ValidateSuccessResult(Result<CreateTeacherDTO> result, string expectedName)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(expectedName);
        }

        private static void ValidateFailResult(Result<CreateTeacherDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
