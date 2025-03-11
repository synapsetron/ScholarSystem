using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Application.MediatR.Students.GetAll;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.UnitTests.MediatR.Students
{
    public class GetAllStudentsHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<GetAllStudentsHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetAllStudentsHandler _handler;

        public GetAllStudentsHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<GetAllStudentsHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetAllStudentsHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWithDTOs_WhenDataExists()
        {
            // Arrange
            var students = GetStudents();
            ConfigureRepositoryToReturn(students);

            // Act
            var result = await _handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty().And.HaveCount(students.Count);
            VerifyLogging(LogLevel.Error, 0);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoStudentsExist()
        {
            // Arrange
            ConfigureRepositoryToReturn(new List<Student>());

            // Act
            var result = await _handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

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
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "An error occurred while retrieving students.");
            VerifyLogging(LogLevel.Error, 1);
        }

        [Fact]
        public async Task Handle_ShouldCallGetAllAsyncOnce()
        {
            // Arrange
            ConfigureRepositoryToReturn(GetStudents());

            // Act
            await _handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.StudentRepository.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()),
                Times.Once);
        }

        // Mock student repository to return students
        private void ConfigureRepositoryToReturn(List<Student> students)
        {
            _mockRepositoryWrapper
                .Setup(repo => repo.StudentRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Student, bool>>>(),
                    It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ReturnsAsync(students);
        }

        // Create test data
        private List<Student> GetStudents() =>
            new()
            {
                new Student { Id = 1, Name = "John Doe", Email = "john@example.com", EnrollmentDate = DateTime.UtcNow },
                new Student { Id = 2, Name = "Jane Smith", Email = "jane@example.com", EnrollmentDate = DateTime.UtcNow }
            };

        private void VerifyLogging(LogLevel level, int times)
        {
            _mockLogger.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Exactly(times));
        }

    }
}
