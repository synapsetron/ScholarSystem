using AutoMapper;
using EntryTask.Application.DTO.Course;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EntryTask.Application.MediatR.Courses.GetByCourseId
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCourseByIdHandler> _logger;

        public GetCourseByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<GetCourseByIdHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<CourseDTO>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var course = await _repositoryWrapper.CourseRepository.GetFirstOrDefaultAsync(
                    predicate: c => c.Id == request.id,
                    include: q => q.Include(c => c.Teacher)
                );

                if (course == null)
                {
                    _logger.LogWarning($"Course with ID {request.id} not found.");
                    return Result.Fail($"Course with ID {request.id} not found.");
                }

                var courseDTO = _mapper.Map<CourseDTO>(course);

                _logger.LogInformation($"Successfully retrieved course: {courseDTO.Title}");
                return Result.Ok(courseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the course.");
                return Result.Fail("An error occurred while retrieving the course.");
            }
        }
    }
}
