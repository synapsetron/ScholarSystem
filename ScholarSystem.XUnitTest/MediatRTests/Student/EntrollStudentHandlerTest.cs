using FluentAssertions;
using FluentResults;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.MediatR.Students.Courses;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.UnitTests.MediatR.Students.Courses
{
    public class EnrollStudentInCourseHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<EnrollStudentInCourseHandler>> _mockLogger;
        private readonly EnrollStudentInCourseHandler _handler;

        public EnrollStudentInCourseHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<EnrollStudentInCourseHandler>>();
            _handler = new EnrollStudentInCourseHandler(_mockRepositoryWrapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenStudentNotFound()
        {
            // Arrange
            SetupStudentRepositoryToReturn(null);
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Student with ID 1 not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCourseNotFound()
        {
            // Arrange
            SetupStudentRepositoryToReturn(new Student { Id = 1 });
            SetupCourseRepositoryToReturn(null);
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Course with ID 1 not found.");
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenStudentAlreadyEnrolled()
        {
            // Arrange
            SetupStudentRepositoryToReturn(new Student { Id = 1 });
            SetupCourseRepositoryToReturn(new Course { Id = 1 });
            SetupEnrollmentRepositoryToReturn(new StudentCourse { StudentId = 1, CourseId = 1, Course = new Course(), Student = new Student() });
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Student 1 is already enrolled in course 1.");
        }

        [Fact]
        public async Task Handle_ShouldEnrollStudent_WhenValid()
        {
            // Arrange
            SetupStudentRepositoryToReturn(new Student { Id = 1 });
            SetupCourseRepositoryToReturn(new Course { Id = 1 });
            SetupEnrollmentRepositoryToReturn(null);
            var request = CreateCommand(1, 1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _mockRepositoryWrapper.Verify(repo => repo.StudentCourseRepository.Create(It.IsAny<StudentCourse>()), Times.Once);
            _mockRepositoryWrapper.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private EnrollStudentInCourseCommand CreateCommand(int studentId, int courseId)
        {
            return new EnrollStudentInCourseCommand(
                new EnrollStudentInCourseDTO
                {
                    StudentId = studentId,
                    CourseId = courseId
                }
            );
        }

        private void SetupStudentRepositoryToReturn(Student? student)
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ReturnsAsync(student);
        }

        private void SetupCourseRepositoryToReturn(Course? course)
        {
            _mockRepositoryWrapper.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Course, bool>>>(), null))
                .ReturnsAsync(course);
        }

        private void SetupEnrollmentRepositoryToReturn(StudentCourse? enrollment)
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentCourseRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StudentCourse, bool>>>(), null))
                .ReturnsAsync(enrollment);
        }
    }
}
