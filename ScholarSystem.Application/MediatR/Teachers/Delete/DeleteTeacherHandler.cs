using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace ScholarSystem.Application.MediatR.Teachers.Delete
{
    public class DeleteTeacherHandler : IRequestHandler<DeleteTeacherCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILogger<DeleteTeacherHandler> _logger;

        public DeleteTeacherHandler(IRepositoryWrapper repositoryWrapper, ILogger<DeleteTeacherHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // get a teacher
                var teacher = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                    predicate: t => t.Id == request.Id,
                    include: query => query.Include(t => t.Courses) // check courses
                );

                if (teacher == null)
                {
                    _logger.LogWarning($"Teacher with ID {request.Id} not found.");
                    return Result.Fail($"Teacher with ID {request.Id} not found.");
                }

                // check if this teacher has a active courses
                if (teacher.Courses.Any())
                {
                    _logger.LogWarning($"Cannot delete teacher ID {request.Id} - they are assigned to courses.");
                    return Result.Fail($"Cannot delete teacher ID {request.Id} - they are assigned to courses.");
                }

                // delete teacher
                 _repositoryWrapper.TeacherRepository.Delete(teacher);
                await _repositoryWrapper.SaveChangesAsync();

                _logger.LogInformation($"Teacher with ID {request.Id} successfully deleted.");
                return Result.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting teacher ID {request.Id}");
                return Result.Fail("An error occurred while deleting the teacher.");
            }
        }
    }
}
