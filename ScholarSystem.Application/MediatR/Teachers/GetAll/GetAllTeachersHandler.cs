using AutoMapper;
using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.Application.MediatR.Teachers.GetAll
{
    public class GetAllTeachersHandler : IRequestHandler<GetAllTeachersQuery, Result<IEnumerable<TeacherDTO>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTeachersHandler> _logger;

        public GetAllTeachersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<GetAllTeachersHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<IEnumerable<TeacherDTO>>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get a list with teacher and their courses
                var teachers = await _repositoryWrapper.TeacherRepository.GetAllAsync(
                    include: query => query.Include(t => t.Courses)
                );

                if (teachers == null || !teachers.Any())
                {
                    _logger.LogWarning("No teachers found in the database.");
                    return Result.Ok(Enumerable.Empty<TeacherDTO>());
                }

                var teacherDTOs = _mapper.Map<IEnumerable<TeacherDTO>>(teachers);

                _logger.LogInformation($"Successfully retrieved {teacherDTOs.Count()} teachers.");
                return Result.Ok(teacherDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching teachers.");
                return Result.Fail("An error occurred while retrieving teachers.");
            }
        }
    }
}
