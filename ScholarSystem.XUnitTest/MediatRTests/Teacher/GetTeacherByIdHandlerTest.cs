using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Application.MediatR.Teachers.GetByTeacherId;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.UnitTests.MediatR.Teachers
{
    public class GetTeacherByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<GetTeacherByIdHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetTeacherByIdHandler _handler;

        public GetTeacherByIdHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<GetTeacherByIdHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Teacher, TeacherDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetTeacherByIdHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenTeacherExists()
        {
            // Arrange
            var teacher = GetTestTeacher();
            SetupTeacherExists(teacher);

            // Act
            var result = await _handler.Handle(new GetTeacherByIdQuery(teacher.Id), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(teacher.Id);
            result.Value.Name.Should().Be(teacher.Name);

            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenTeacherDoesNotExist()
        {
            // Arrange
            SetupTeacherDoesNotExist();

            // Act
            var result = await _handler.Handle(new GetTeacherByIdQuery(999), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "Teacher with ID 999 not found.");

            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenExceptionThrown()
        {
            // Arrange
            SetupRepositoryThrowsException();

            // Act
            var result = await _handler.Handle(new GetTeacherByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "An error occurred while retrieving the teacher.");

            VerifyLogging(LogLevel.Error, 1);
        }

        [Fact]
        public async Task Handle_ShouldCallGetFirstOrDefaultAsyncOnce()
        {
            // Arrange
            SetupTeacherExists(GetTestTeacher());

            // Act
            await _handler.Handle(new GetTeacherByIdQuery(1), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()),
                Times.Once);
        }

        // Mock teacher repository return
        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Teacher, bool>>>(),
                    It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ReturnsAsync(teacher);
        }

        private void SetupTeacherDoesNotExist()
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Teacher, bool>>>(),
                    It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupRepositoryThrowsException()
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Teacher, bool>>>(),
                    It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ThrowsAsync(new Exception("Database error"));
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

        private static Teacher GetTestTeacher() =>
            new()
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@email.com",
                Courses = new List<Course>()
            };
    }
}
