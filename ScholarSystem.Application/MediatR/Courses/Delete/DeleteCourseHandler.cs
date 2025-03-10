using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace ScholarSystem.Application.MediatR.Courses.Delete
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILogger<DeleteCourseHandler> _logger;

        public DeleteCourseHandler(IRepositoryWrapper repositoryWrapper, ILogger<DeleteCourseHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _repositoryWrapper.CourseRepository.GetFirstOrDefaultAsync(
                    predicate: c => c.Id == request.id, 
                    include: query => query.Include(c => c.StudentCourses)
                );

                if (course == null)
                {
                    _logger.LogWarning($"Course with ID {request.id} not found.");
                    return Result.Fail($"Course with ID {request.id} not found.");
                }

                // Delete course, cascade delete will delete all related entities
                _repositoryWrapper.CourseRepository.Delete(course);

                await _repositoryWrapper.SaveChangesAsync();

                _logger.LogInformation($"Course with ID {request.id} successfully deleted.");
                return Result.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting course ID {request.id}");
                return Result.Fail("An error occurred while deleting the course.");
            }
        }
    }
}
