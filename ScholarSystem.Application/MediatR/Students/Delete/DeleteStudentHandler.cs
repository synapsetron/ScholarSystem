using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace ScholarSystem.Application.MediatR.Students.Delete
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, Result<Unit>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILogger<DeleteStudentHandler> _logger;

        public DeleteStudentHandler(IRepositoryWrapper repositoryWrapper, ILogger<DeleteStudentHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<Unit>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // find student by ID and include StudentCourses
                var student = await _repositoryWrapper.StudentRepository.GetFirstOrDefaultAsync(
                    predicate: s => s.Id == request.id,
                    include: query => query.Include(s => s.StudentCourses));

                if (student == null)
                {
                    _logger.LogWarning($"Student with ID {request.id} not found.");
                    return Result.Fail($"Student with ID {request.id} not found.");
                }

                // delete student, cascade delete will delete StudentCourses
                _repositoryWrapper.StudentRepository.Delete(student);
                await _repositoryWrapper.SaveChangesAsync();

                _logger.LogInformation($"Student with ID {request.id} successfully deleted.");
                return Result.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting student ID {request.id}");
                return Result.Fail("An error occurred while deleting the student.");
            }
        }
    }
}
