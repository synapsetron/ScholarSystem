using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Application.MediatR.Students.GetByStudentId;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.UnitTests.MediatR.Students
{
    public class GetByStudentIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<GetByStudentIdHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly GetByStudentIdHandler _handler;

        public GetByStudentIdHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<GetByStudentIdHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>());
            _mapper = config.CreateMapper();

            _handler = new GetByStudentIdHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenStudentExists()
        {
            // Arrange
            var student = GetTestStudent();
            SetupStudentExists(student);

            // Act
            var result = await _handler.Handle(new GetByStudentIdQuery(student.Id), CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, student.Id);
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenStudentDoesNotExist()
        {
            // Arrange
            SetupStudentDoesNotExist();

            // Act
            var result = await _handler.Handle(new GetByStudentIdQuery(999), CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Student with ID 999 not found.");
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailResult_WhenExceptionThrown()
        {
            // Arrange
            SetupRepositoryThrowsException();

            // Act
            var result = await _handler.Handle(new GetByStudentIdQuery(1), CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while retrieving the student.");
            VerifyLogging(LogLevel.Error, 1);
        }

        [Fact]
        public async Task Handle_ShouldCallGetFirstOrDefaultAsyncOnce()
        {
            // Arrange
            SetupStudentExists(GetTestStudent());

            // Act
            await _handler.Handle(new GetByStudentIdQuery(1), CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()),
                Times.Once);
        }
        private void SetupStudentExists(Student student)
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ReturnsAsync(student);
        }

        private void SetupStudentDoesNotExist()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ReturnsAsync((Student)null!);
        }

        private void SetupRepositoryThrowsException()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(),
                It.IsAny<Func<IQueryable<Student>, IIncludableQueryable<Student, object>>>()))
                .ThrowsAsync(new Exception("Database error"));
        }

        private static void ValidateSuccessResult(Result<StudentDTO> result, int expectedId)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(expectedId);
        }

        private static void ValidateFailResult(Result<StudentDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }

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

        private static Student GetTestStudent() =>
            new()
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                EnrollmentDate = DateTime.UtcNow,
                StudentCourses = new List<StudentCourse>()
            };
    }
}
