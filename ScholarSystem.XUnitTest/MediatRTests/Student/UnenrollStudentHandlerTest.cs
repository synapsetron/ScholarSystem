using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.MediatR.Students.Courses;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.UnitTests.MediatR.Students.Courses
{
    public class UnenrollStudentFromCourseHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<UnenrollStudentFromCourseHandler>> _mockLogger;
        private readonly UnenrollStudentFromCourseHandler _handler;

        public UnenrollStudentFromCourseHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<UnenrollStudentFromCourseHandler>>();
            _handler = new UnenrollStudentFromCourseHandler(_mockRepositoryWrapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenEnrollmentNotFound()
        {
            // Arrange
            SetupEnrollmentRepositoryToReturn(null);
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No enrollment found for Student 1 in Course 1.");
        }

        [Fact]
        public async Task Handle_ShouldUnenrollStudent_WhenEnrollmentExists()
        {
            // Arrange
            var enrollment = new StudentCourse { StudentId = 1, CourseId = 1 };
            SetupEnrollmentRepositoryToReturn(enrollment);
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _mockRepositoryWrapper.Verify(repo => repo.StudentCourseRepository.Delete(enrollment), Times.Once);
            _mockRepositoryWrapper.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLogWarning_WhenEnrollmentNotFound()
        {
            // Arrange
            SetupEnrollmentRepositoryToReturn(null);
            var request = CreateCommand(1, 1);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("No enrollment found for Student 1 in Course 1.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldLogInformation_WhenUnenrollmentIsSuccessful()
        {
            // Arrange
            var enrollment = new StudentCourse { StudentId = 1, CourseId = 1 };
            SetupEnrollmentRepositoryToReturn(enrollment);
            var request = CreateCommand(1, 1);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Student 1 successfully unenrolled from course 1.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        private UnenrollStudentFromCourseCommand CreateCommand(int studentId, int courseId)
        {
            return new UnenrollStudentFromCourseCommand(
                new UnenrollStudentFromCourseDTO
                {
                    StudentId = studentId,
                    CourseId = courseId
                }
            );
        }

        private void SetupEnrollmentRepositoryToReturn(StudentCourse? enrollment)
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentCourseRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StudentCourse, bool>>>(), null))
                .ReturnsAsync(enrollment);
        }
    }
}