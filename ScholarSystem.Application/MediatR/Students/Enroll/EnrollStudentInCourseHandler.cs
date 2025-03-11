using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScholarSystem.Application.MediatR.Students.Courses
{
    public class EnrollStudentInCourseHandler : IRequestHandler<EnrollStudentInCourseCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILogger<EnrollStudentInCourseHandler> _logger;

        public EnrollStudentInCourseHandler(IRepositoryWrapper repositoryWrapper, ILogger<EnrollStudentInCourseHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(EnrollStudentInCourseCommand request, CancellationToken cancellationToken)
        {
            var studentId = request.EnrollStudentDTO.StudentId;
            var courseId = request.EnrollStudentDTO.CourseId;

            // check if student exists
            var student = await _repositoryWrapper.StudentRepository.GetFirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                _logger.LogWarning($"Student with ID {studentId} not found.");
                return Result.Fail($"Student with ID {studentId} not found.");
            }

            // check if course exists
            var course = await _repositoryWrapper.CourseRepository.GetFirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                _logger.LogWarning($"Course with ID {courseId} not found.");
                return Result.Fail($"Course with ID {courseId} not found.");
            }

            // сheck if student is already enrolled in the course
            var existingEnrollment = await _repositoryWrapper.StudentCourseRepository.GetFirstOrDefaultAsync(
                sc => sc.StudentId == studentId && sc.CourseId == courseId);
            if (existingEnrollment != null)
            {
                _logger.LogWarning($"Student {studentId} is already enrolled in course {courseId}.");
                return Result.Fail($"Student {studentId} is already enrolled in course {courseId}.");
            }

           
            var studentCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow
            };

            _repositoryWrapper.StudentCourseRepository.Create(studentCourse);
            await _repositoryWrapper.SaveChangesAsync();

            _logger.LogInformation($"Student {studentId} successfully enrolled in course {courseId}.");
            return Result.Ok(Unit.Value);
        }
    }
}
