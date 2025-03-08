
using AutoMapper;
using EntryTask.Application.DTO.Course;
using EntryTask.Application.Interfaces.Logging;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.GetAll
{
    public class GetAllCoursesHandler : IRequestHandler<GetAllCoursesQuery, Result<IEnumerable<CourseDTO>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        public GetAllCoursesHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerService logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(_repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Result<IEnumerable<CourseDTO>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();

                if (courses == null || !courses.Any())
                {
                    _logger.LogWarning("No courses found in the database.");
                    return Result.Ok(Enumerable.Empty<CourseDTO>());
                }

                var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(courses);

                _logger.LogInformation($"Successfully retrieved {courseDTOs.Count()} courses.");
                return Result.Ok(courseDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching courses.");
                return Result.Fail("An error occurred while retrieving courses.");
            }

        }
    }
}

