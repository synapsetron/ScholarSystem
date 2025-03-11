using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Application.MediatR.Teachers.GetAll;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Query;


namespace ScholarSystem.UnitTests.MediatR.Teachers
{
    public class GetAllTeachersHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<GetAllTeachersHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetAllTeachersHandler _handler;

        public GetAllTeachersHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<GetAllTeachersHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Teacher, TeacherDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetAllTeachersHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWithDTOs_WhenTeachersExist()
        {
            // Arrange
            var teachers = GetTeachers();
            SetupTeachersExist(teachers);

            // Act
            var result = await _handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty().And.HaveCount(teachers.Count);
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTeachersExist()
        {
            // Arrange
            SetupTeachersExist(new List<Teacher>());

            // Act
            var result = await _handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenExceptionThrown()
        {
            // Arrange
            SetupRepositoryThrowsException();

            // Act
            var result = await _handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "An error occurred while retrieving teachers.");
            VerifyLogging(LogLevel.Error, 1);
        }

        [Fact]
        public async Task Handle_ShouldCallGetAllAsyncOnce()
        {
            // Arrange
            SetupTeachersExist(GetTeachers());

            // Act
            await _handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.TeacherRepository.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()),
                Times.Once);
        }

        // 🔹 Helper Methods

        private static List<Teacher> GetTeachers() =>
            new()
            {
                new Teacher { Id = 1, Name = "John Smith", Email = "john.smith@email.com", Courses = new List<Course>() },
                new Teacher { Id = 2, Name = "Sarah Johnson", Email = "sarah.johnson@email.com", Courses = new List<Course>() }
            };

        private void SetupTeachersExist(List<Teacher> teachers)
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>>(),
                It.IsAny<Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>>()))
                .ReturnsAsync(teachers);
        }

        private void SetupRepositoryThrowsException()
        {
            _mockRepository.Setup(repo => repo.TeacherRepository.GetAllAsync(
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
    }
}
