using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Application.MediatR.Courses.Create;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;

namespace ScholarSystem.UnitTests.MediatR.Courses
{
    public class CreateCourseHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<CreateCourseHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly CreateCourseHandler _handler;

        public CreateCourseHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<CreateCourseHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CreateCourseDTO>().ReverseMap());
            _mapper = config.CreateMapper();

            _handler = new CreateCourseHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateCourse_WhenTeacherExists()
        {
            // Arrange
            var request = new CreateCourseCommand(GetCreateCourseDTO());
            var teacher = GetTestTeacher();
            var course = _mapper.Map<Course>(request.courseDTO);

            SetupTeacherExists(teacher);
            SetupCourseCreation(course);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, request.courseDTO.Title);
            VerifyRepositoryCalls();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenTeacherDoesNotExist()
        {
            // Arrange
            var request = new CreateCourseCommand(GetCreateCourseDTO());

            SetupTeacherDoesNotExist();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Teacher with ID {request.courseDTO.TeacherId} not found.");
            VerifyNoCourseCreation();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateCourseCommand(GetCreateCourseDTO());

            SetupTeacherThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while creating the course.");
            VerifyNoCourseCreation();
            VerifyLogging(LogLevel.Error, 1);
        }


        private static CreateCourseDTO GetCreateCourseDTO() =>
            new() { Title = "Software Architecture", Description = "Intro to architectures", Credits = 5, TeacherId = 1 };

        private static Teacher GetTestTeacher() =>
            new() { Id = 1, Name = "Test Teacher" };

        private void SetupTeacherExists(Teacher teacher)
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync(teacher);
        }

        private void SetupTeacherDoesNotExist()
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ReturnsAsync((Teacher)null!);
        }

        private void SetupTeacherThrowsException()
        {
            _mockRepository.Setup(x => x.TeacherRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupCourseCreation(Course course)
        {
            _mockRepository.Setup(repo => repo.CourseRepository.Create(It.IsAny<Course>()))
                .Returns(course);
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyRepositoryCalls()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Create(It.IsAny<Course>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoCourseCreation()
        {
            _mockRepository.Verify(repo => repo.CourseRepository.Create(It.IsAny<Course>()), Times.Never);
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

        private static void ValidateSuccessResult(Result<CreateCourseDTO> result, string expectedTitle)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Title.Should().Be(expectedTitle);
        }

        private static void ValidateFailResult(Result<CreateCourseDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
