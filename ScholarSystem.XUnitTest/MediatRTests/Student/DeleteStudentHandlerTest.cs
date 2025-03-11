using FluentAssertions;
using Moq;
using ScholarSystem.Application.MediatR.Students.Delete;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;

namespace ScholarSystem.UnitTests.MediatR.Students
{
    public class DeleteStudentHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<DeleteStudentHandler>> _mockLogger;
        private readonly DeleteStudentHandler _handler;

        public DeleteStudentHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<DeleteStudentHandler>>();
            _handler = new DeleteStudentHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteStudent_WhenStudentExists()
        {
            // Arrange
            var request = new DeleteStudentCommand(1);
            var student = GetTestStudent();

            SetupStudentExists(student);
            SetupSuccessfulDeletion();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result);
            VerifyStudentDeletion();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenStudentDoesNotExist()
        {
            // Arrange
            var request = new DeleteStudentCommand(1);

            SetupStudentDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Student with ID {request.id} not found.");
            VerifyNoStudentDeletion();
            VerifyLogging(LogLevel.Warning, 1);
        }
        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new DeleteStudentCommand(1);
            SetupStudentThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while deleting the student.");
            VerifyNoStudentDeletion();
            VerifyLogging(LogLevel.Error, 1);
        }
        private static Student GetTestStudent() =>
        new() { Id = 1, Name = "Test Student", StudentCourses = new List<StudentCourse>() };

        private void SetupStudentExists(Student student)
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ReturnsAsync(student);
        }

        private void SetupStudentDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ReturnsAsync((Student)null!);
        }

        private void SetupStudentThrowsException()
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupSuccessfulDeletion()
        {
            _mockRepository.Setup(repo => repo.StudentRepository.Delete(It.IsAny<Student>()));
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyStudentDeletion()
        {
            _mockRepository.Verify(repo => repo.StudentRepository.Delete(It.IsAny<Student>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoStudentDeletion()
        {
            _mockRepository.Verify(repo => repo.StudentRepository.Delete(It.IsAny<Student>()), Times.Never);
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
