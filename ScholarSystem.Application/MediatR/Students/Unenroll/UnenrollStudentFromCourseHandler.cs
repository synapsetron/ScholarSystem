using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScholarSystem.Application.MediatR.Students.Courses
{
    public class UnenrollStudentFromCourseHandler : IRequestHandler<UnenrollStudentFromCourseCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILogger<UnenrollStudentFromCourseHandler> _logger;

        public UnenrollStudentFromCourseHandler(IRepositoryWrapper repositoryWrapper, ILogger<UnenrollStudentFromCourseHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(UnenrollStudentFromCourseCommand request, CancellationToken cancellationToken)
        {
            var studentId = request.UnenrollStudentDTO.StudentId;
            var courseId = request.UnenrollStudentDTO.CourseId;

            // check if  exists
            var studentCourse = await _repositoryWrapper.StudentCourseRepository.GetFirstOrDefaultAsync(
                sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (studentCourse == null)
            {
                _logger.LogWarning($"No enrollment found for Student {studentId} in Course {courseId}.");
                return Result.Fail($"No enrollment found for Student {studentId} in Course {courseId}.");
            }

            // delete studentCourse
            _repositoryWrapper.StudentCourseRepository.Delete(studentCourse);
            await _repositoryWrapper.SaveChangesAsync();

            _logger.LogInformation($"Student {studentId} successfully unenrolled from course {courseId}.");
            return Result.Ok(Unit.Value);
        }
    }
}
