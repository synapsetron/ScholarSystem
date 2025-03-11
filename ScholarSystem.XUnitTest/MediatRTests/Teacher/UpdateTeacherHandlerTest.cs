using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Application.MediatR.Teachers.Update;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;

namespace ScholarSystem.UnitTests.MediatR.Teachers
{
    public class UpdateTeacherHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<UpdateTeacherHandler>> _mockLogger;
        private readonly UpdateTeacherHandler _handler;
        private readonly IMapper _mapper;

        public UpdateTeacherHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<UpdateTeacherHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Teacher, TeacherDTO>().ReverseMap());
            _mapper = config.CreateMapper();

            _handler = new UpdateTeacherHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTeacher_WhenTeacherExists()
        {
            // Arrange
            var request = new UpdateTeacherCommand(GetUpdateTeacherDTO());
            var existingTeacher = GetTestTeacher();

            SetupTeacherExists(existingTeacher);

          
            SetupEmailAlreadyExists(null, existingTeacher);

            SetupSuccessfulUpdate();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, request.updateTeacherDTO.Name);
            VerifyTeacherUpdate();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherDoesNotExist()
        {
            // Arrange
            var request = new UpdateTeacherCommand(GetUpdateTeacherDTO());

            SetupTeacherDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Teacher with ID {request.updateTeacherDTO.Id} not found.");
            VerifyNoTeacherUpdate();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new UpdateTeacherCommand(GetUpdateTeacherDTO());
            var existingTeacher = GetTestTeacher();  // сurrent teacher
            var anotherTeacher = GetAnotherTestTeacher(); // teache with the same email

            SetupTeacherExists(existingTeacher);
            SetupEmailAlreadyExists(anotherTeacher, existingTeacher);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Another teacher with email {request.updateTeacherDTO.Email} already exists.");
            VerifyNoTeacherUpdate();
            VerifyLogging(LogLevel.Warning, 1);
        }


        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new UpdateTeacherCommand(GetUpdateTeacherDTO());

            SetupTeacherThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while updating the teacher.");
            VerifyNoTeacherUpdate();
            VerifyLogging(LogLevel.Error, 1);
        }

        private static UpdateTeacherDTO GetUpdateTeacherDTO() =>
            new() { Id = 1, Name = "Updated Teacher", Email = "updated@email.com" };

        private static Teacher GetTestTeacher() =>
            new() { Id = 1, Name = "Original Teacher", Email = "original@email.com" };

        private static Teacher GetAnotherTestTeacher() =>
            new() { Id = 2, Name = "Another Teacher", Email = "updated@email.com" };

        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Teacher, bool>>>(predicate =>
                    predicate.Compile().Invoke(teacher)),
                null))
                .ReturnsAsync(teacher);
        }

        private void SetupEmailAlreadyExists(Teacher? existingTeacher, Teacher updatingTeacher)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Teacher, bool>>>(predicate =>
                    existingTeacher != null &&
                    predicate.Compile().Invoke(existingTeacher) &&
                    existingTeacher.Id != updatingTeacher.Id),
                null))
                .ReturnsAsync(existingTeacher);
        }


        private void SetupTeacherDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupTeacherThrowsException()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupSuccessfulUpdate()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.Update(It.IsAny<Teacher>()));
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyTeacherUpdate()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Update(It.IsAny<Teacher>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoTeacherUpdate()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Update(It.IsAny<Teacher>()), Times.Never);
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

        private static void ValidateSuccessResult(Result<TeacherDTO> result, string expectedName)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(expectedName);
        }

        private static void ValidateFailResult(Result<TeacherDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
