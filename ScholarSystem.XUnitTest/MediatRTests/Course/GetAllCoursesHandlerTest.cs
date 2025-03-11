using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Application.Interfaces.Logging;
using ScholarSystem.Application.MediatR.Courses.GetAll;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;


namespace ScholarSystem.UnitTests.MediatR.Courses
{
    public class GetAllCoursesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetAllCoursesHandler _handler;

        public GetAllCoursesHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetAllCoursesHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        
        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWithDTOs_WhenDataExists()
        {
            // Arrange
            var courses = GetCourses();
            ConfigureRepositoryToReturn(courses);

            // Act
            var result = await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty().And.HaveCount(courses.Count);
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<object>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCoursesExist()
        {
            // Arrange
            ConfigureRepositoryToReturn(new List<Course>());

            // Act
            var result = await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
            _mockLogger.Verify(logger => logger.LogWarning(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenExceptionThrown()
        {
            // Arrange
            _mockRepositoryWrapper.Setup(repo => repo.CourseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "An error occurred while retrieving courses.");
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallGetAllAsyncOnce()
        {
            // Arrange
            ConfigureRepositoryToReturn(GetCourses());

            // Act
            await _handler.Handle(new GetAllCoursesQuery(), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.CourseRepository.GetAllAsync(
                It.IsAny<Expression<Func<Course, bool>>>(),
                It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()),
                Times.Once);
        }

        //mock course repository to return courses
        private void ConfigureRepositoryToReturn(List<Course> courses)
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.CourseRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Course, bool>>>(),
                    It.IsAny<Func<IQueryable<Course>, IIncludableQueryable<Course, object>>>()))
                .ReturnsAsync(courses);
        }

        // Create test data
        private List<Course> GetCourses() =>
            new()
            {
                new Course { Id = 1, Title = "Math", Description = "Basic math", Credits = 5, TeacherId = 1 },
                new Course { Id = 2, Title = "Physics", Description = "Intro to physics", Credits = 4, TeacherId = 2 }
            };
    }
}
