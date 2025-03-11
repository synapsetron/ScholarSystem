using FluentAssertions;
using Moq;
using ScholarSystem.Application.MediatR.Courses.Delete;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;

namespace ScholarSystem.UnitTests.MediatR.Courses
{
    public class DeleteCourseHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<DeleteCourseHandler>> _mockLogger;
        private readonly DeleteCourseHandler _handler;

        public DeleteCourseHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<DeleteCourseHandler>>();

            _handler = new DeleteCourseHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCourse_WhenCourseExists()
        {
            // Arrange
            var request = new DeleteCourseCommand(1);
            var course = GetTestCourse();

            SetupCourseExists(course);
            SetupSuccessfulDeletion();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result);
            VerifyCourseDeletion();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenCourseDoesNotExist()
        {
            // Arrange
            var request = new DeleteCourseCommand(1);

            SetupCourseDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Course with ID {request.id} not found.");
            VerifyNoCourseDeletion();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new DeleteCourseCommand(1);
            SetupCourseThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while deleting the course.");
            VerifyNoCourseDeletion();
            VerifyLogging(LogLevel.Error, 1);
        }

        private static Course GetTestCourse() =>
            new() { Id = 1, Title = "Test Course", StudentCourses = new List<StudentCourse>() };

        private void SetupCourseExists(Course course)
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ReturnsAsync(course);
        }

        private void SetupCourseDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ReturnsAsync((Course)null!);
        }

        private void SetupCourseThrowsException()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupSuccessfulDeletion()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.Delete(It.IsAny<Course>()));
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyCourseDeletion()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Delete(It.IsAny<Course>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoCourseDeletion()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Delete(It.IsAny<Course>()), Times.Never);
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

        private static void ValidateSuccessResult(Result<Unit> result)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        private static void ValidateFailResult(Result<Unit> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
