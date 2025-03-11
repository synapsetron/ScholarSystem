using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Application.MediatR.Courses.GetByCourseId;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.UnitTests.MediatR.Courses
{
    public class GetCourseByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<GetCourseByIdHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetCourseByIdHandler _handler;

        public GetCourseByIdHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<GetCourseByIdHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetCourseByIdHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenCourseExists()
        {
            // Arrange
            var course = GetTestCourse();
            ConfigureRepositoryToReturn(course);

            // Act
            var result = await _handler.Handle(new GetCourseByIdQuery(course.Id), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(course.Id);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null, // verify that exception is not logged
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenCourseDoesNotExist()
        {
            // Arrange
            ConfigureRepositoryToReturn(null);

            // Act
            var result = await _handler.Handle(new GetCourseByIdQuery(999), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "Course with ID 999 not found.");

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    null, // verify that exception is not logged
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenExceptionThrown()
        {
            // Arrange
            _mockRepositoryWrapper.Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(new GetCourseByIdQuery(1), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "An error occurred while retrieving the course.");

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), // verify that exception is logged
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallGetFirstOrDefaultAsyncOnce()
        {
            // Arrange
            ConfigureRepositoryToReturn(GetTestCourse());

            // Act
            await _handler.Handle(new GetCourseByIdQuery(1), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()),
                Times.Once);
        }

        // mock return of a course
        private void ConfigureRepositoryToReturn(Course? course)
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.CourseRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Course, bool>>>(),
                    It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ReturnsAsync(course);
        }

        // test data
        private Course GetTestCourse() =>
            new()
            {
                Id = 1,
                Title = "Software Engineering",
                Description = "Course on modern software development practices.",
                Credits = 5,
                TeacherId = 1
            };
    }
}
