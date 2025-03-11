using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Application.MediatR.Students.Update;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ScholarSystem.UnitTests.MediatR.Students
{
    public class UpdateStudentHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILogger<UpdateStudentHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly UpdateStudentHandler _handler;

        public UpdateStudentHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<UpdateStudentHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, UpdateStudentDTO>());
            _mapper = config.CreateMapper();

            _handler = new UpdateStudentHandler(_mockRepositoryWrapper.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenStudentExists()
        {
            // Arrange
            var student = GetTestStudent();
            SetupStudentExists(student);
            var request = CreateCommand(student);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, student.Id);
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenStudentDoesNotExist()
        {
            // Arrange
            SetupStudentDoesNotExist();
            var request = CreateCommand(GetTestStudent());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "Student with ID 1 not found.");
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldUpdateStudent_WhenValid()
        {
            // Arrange
            var student = GetTestStudent();
            SetupStudentExists(student);
            var request = CreateCommand(student);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.StudentRepository.Update(student), Times.Once);
            _mockRepositoryWrapper.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenExceptionThrown()
        {
            // Arrange
            SetupRepositoryThrowsException();
            var request = CreateCommand(GetTestStudent());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while updating the student.");
            VerifyLogging(LogLevel.Error, 1);
        }

        [Fact]
        public async Task Handle_ShouldCallGetFirstOrDefaultAsyncOnce()
        {
            // Arrange
            SetupStudentExists(GetTestStudent());
            var request = CreateCommand(GetTestStudent());

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockRepositoryWrapper.Verify(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null),
                Times.Once);
        }

        private static UpdateStudentCommand CreateCommand(Student student)
        {
            return new UpdateStudentCommand(
                new UpdateStudentDTO
                {
                    Id = student.Id,
                    Name = "Updated Name",
                    Email = "updated@example.com",
                    EnrollmentDate = student.EnrollmentDate
                }
            );
        }

        private void SetupStudentExists(Student student)
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ReturnsAsync(student);
        }

        private void SetupStudentDoesNotExist()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ReturnsAsync((Student)null!);
        }

        private void SetupRepositoryThrowsException()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private static void ValidateSuccessResult(Result<UpdateStudentDTO> result, int expectedId)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(expectedId);
        }

        private static void ValidateFailResult(Result<UpdateStudentDTO> result, string expectedError)
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
