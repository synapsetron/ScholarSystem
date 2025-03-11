using FluentAssertions;
using Moq;
using ScholarSystem.Application.MediatR.Teachers.Delete;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;

namespace ScholarSystem.UnitTests.MediatR.Teachers
{
    public class DeleteTeacherHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<DeleteTeacherHandler>> _mockLogger;
        private readonly DeleteTeacherHandler _handler;

        public DeleteTeacherHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<DeleteTeacherHandler>>();

            _handler = new DeleteTeacherHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteTeacher_WhenTeacherExistsAndHasNoCourses()
        {
            // Arrange
            var request = new DeleteTeacherCommand(1);
            var teacher = GetTestTeacher(hasCourses: false);

            SetupTeacherExists(teacher);
            SetupSuccessfulDeletion();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result);
            VerifyTeacherDeletion();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherDoesNotExist()
        {
            // Arrange
            var request = new DeleteTeacherCommand(1);

            SetupTeacherDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Teacher with ID {request.Id} not found.");
            VerifyNoTeacherDeletion();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherHasCourses()
        {
            // Arrange
            var request = new DeleteTeacherCommand(1);
            var teacher = GetTestTeacher(hasCourses: true);

            SetupTeacherExists(teacher);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Cannot delete teacher ID {request.Id} - they are assigned to courses.");
            VerifyNoTeacherDeletion();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new DeleteTeacherCommand(1);
            SetupTeacherThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while deleting the teacher.");
            VerifyNoTeacherDeletion();
            VerifyLogging(LogLevel.Error, 1);
        }

        // 🔹 Helper methods

        private static Teacher GetTestTeacher(bool hasCourses) =>
            new()
            {
                Id = 1,
                Name = "Test Teacher",
                Email = "teacher@example.com",
                Courses = hasCourses ? new List<Course> { new Course { Id = 1, Title = "Math" } } : new List<Course>()
            };

        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ReturnsAsync(teacher);
        }

        private void SetupTeacherDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupTeacherThrowsException()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupSuccessfulDeletion()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.Delete(It.IsAny<Teacher>()));
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyTeacherDeletion()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Delete(It.IsAny<Teacher>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoTeacherDeletion()
        {
            _mockRepository.Verify(repo => repo.TeacherRepository.Delete(It.IsAny<Teacher>()), Times.Never);
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
