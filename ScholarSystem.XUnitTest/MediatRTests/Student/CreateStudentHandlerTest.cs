using AutoMapper;
using FluentAssertions;
using Moq;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Application.MediatR.Students.Create;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentResults;

namespace ScholarSystem.UnitTests.MediatR.Students
{
    public class CreateStudentHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILogger<CreateStudentHandler>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly CreateStudentHandler _handler;

        public CreateStudentHandlerTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILogger<CreateStudentHandler>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, CreateStudentDTO>().ReverseMap());
            _mapper = config.CreateMapper();

            _handler = new CreateStudentHandler(_mockRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateStudent_WhenEmailDoesNotExist()
        {
            // Arrange
            var request = new CreateStudentCommand(GetCreateStudentDTO());
            var student = _mapper.Map<Student>(request.studentDTO);

            SetupStudentDoesNotExist();
            SetupStudentCreation(student);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateSuccessResult(result, request.studentDTO.Name);
            VerifyRepositoryCalls();
            VerifyLogging(LogLevel.Information, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new CreateStudentCommand(GetCreateStudentDTO());
            var existingStudent = GetTestStudent();

            SetupStudentExists(existingStudent);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, $"Student with email {request.studentDTO.Email} already exists.");
            VerifyNoStudentCreation();
            VerifyLogging(LogLevel.Warning, 1);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateStudentCommand(GetCreateStudentDTO());

            SetupRepositoryThrowsException();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ValidateFailResult(result, "An error occurred while creating the student.");
            VerifyNoStudentCreation();
            VerifyLogging(LogLevel.Error, 1);
        }

        private static CreateStudentDTO GetCreateStudentDTO() =>
            new() { Name = "John Doe", Email = "john@example.com", EnrollmentDate = DateTime.UtcNow };

        private static Student GetTestStudent() =>
            new() { Id = 1, Name = "John Doe", Email = "john@example.com", EnrollmentDate = DateTime.UtcNow };

        private void SetupStudentExists(Student student)
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ReturnsAsync(student);
        }

        private void SetupStudentDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ReturnsAsync((Student)null!);
        }

        private void SetupRepositoryThrowsException()
        {
            _mockRepository.Setup(repo => repo.StudentRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Student, bool>>>(), null))
                .ThrowsAsync(new Exception("Database error"));
        }

        private void SetupStudentCreation(Student student)
        {
            _mockRepository.Setup(repo => repo.StudentRepository.Create(It.IsAny<Student>()))
                .Returns(student);
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }

        private void VerifyRepositoryCalls()
        {
            _mockRepository.Verify(repo => repo.StudentRepository.Create(It.IsAny<Student>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void VerifyNoStudentCreation()
        {
            _mockRepository.Verify(repo => repo.StudentRepository.Create(It.IsAny<Student>()), Times.Never);
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

        private static void ValidateSuccessResult(Result<CreateStudentDTO> result, string expectedName)
        {
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Name.Should().Be(expectedName);
        }

        private static void ValidateFailResult(Result<CreateStudentDTO> result, string expectedError)
        {
            result.Should().NotBeNull();
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == expectedError);
        }
    }
}
