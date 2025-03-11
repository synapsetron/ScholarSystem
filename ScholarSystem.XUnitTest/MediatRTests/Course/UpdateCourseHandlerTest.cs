using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Application.MediatR.Courses.Update;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FluentResults;

namespace ScholarSystem.UnitTests.MediatR.Courses
{
    public class UpdateCourseHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<UpdateCourseHandler>> _mockLogger;
        private readonly UpdateCourseHandler _handler;
        private readonly IMapper _mapper;

        public UpdateCourseHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<UpdateCourseHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>().ReverseMap());
            _mapper = config.CreateMapper();

            _handler = new UpdateCourseHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCourse_WhenCourseExists()
        {
            // Arrange
            var request = new UpdateCourseCommand(GetUpdateCourseDTO());
            var existingCourse = GetTestCourse();

            SetupCourseExists(existingCourse);
            SetupSuccessfulUpdate();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, request.UpdateCourseDTO.Title);
            VerifyCourseUpdate();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenCourseDoesNotExist()
        {
            // Arrange
            var request = new UpdateCourseCommand(GetUpdateCourseDTO());

            SetupCourseDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Course with ID {request.UpdateCourseDTO.Id} not found.");
            VerifyNoCourseUpdate();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherDoesNotExist()
        {
            // Arrange
            var request = new UpdateCourseCommand(GetUpdateCourseDTO());
            var existingCourse = GetTestCourse();

            SetupCourseExists(existingCourse);
            SetupTeacherDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Teacher with ID {request.UpdateCourseDTO.TeacherId} not found.");
            VerifyNoCourseUpdate();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new UpdateCourseCommand(GetUpdateCourseDTO());

            SetupCourseThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while updating the course.");
            VerifyNoCourseUpdate();
            VerifyLogging(LogLevel.Error, 1);
        }


        private static UpdateCourseDTO GetUpdateCourseDTO() =>
            new() { Id = 1, Title = "Updated Course", Description = "Updated Description", Credits = 5, TeacherId = 2 };

        private static Course GetTestCourse() =>
            new() { Id = 1, Title = "Old Course", Description = "Old Description", Credits = 3, TeacherId = 1 };

        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync(teacher);
        }

        private void SetupCourseExists(Course course)
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(), null))
                .ReturnsAsync(course);

            // if teacher id changed, mock a new teacher
            if (course.TeacherId != GetUpdateCourseDTO().TeacherId)
            {
                SetupTeacherExists(new Teacher { Id = GetUpdateCourseDTO().TeacherId, Name = "New Teacher" });
            }
        }


        private void SetupCourseDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(), null))
                .ReturnsAsync((Course)null!);
        }

        private void SetupTeacherDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupCourseThrowsException()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupSuccessfulUpdate()
        {
            _mockRepository.Setup(repo => repo.CourseRepository.Update(It.IsAny<Course>()));
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyCourseUpdate()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Update(It.IsAny<Course>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoCourseUpdate()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Update(It.IsAny<Course>()), Times.Never);
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

        private static void ValidateSuccessResult(Result<CourseDTO> result, string expectedTitle)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be(expectedTitle);
        }

        private static void ValidateFailResult(Result<CourseDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
