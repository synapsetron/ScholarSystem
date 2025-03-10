using AutoMapper;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;


namespace ScholarSystem.Application.MediatR.Courses.Update
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Result<CourseDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCourseHandler> _logger;

        public UpdateCourseHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<UpdateCourseHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<CourseDTO>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // check if course exist
                var existingCourse = await _repositoryWrapper.CourseRepository.GetFirstOrDefaultAsync(
                    predicate: c => c.Id == request.UpdateCourseDTO.Id);

                if (existingCourse == null)
                {
                    _logger.LogWarning($"Course with ID {request.UpdateCourseDTO.Id} not found.");
                    return Result.Fail($"Course with ID {request.UpdateCourseDTO.Id} not found.");
                }

                // Check ,does a  new teacher exist ? (if  changed  `TeacherId`)
                if (existingCourse.TeacherId != request.UpdateCourseDTO.TeacherId)
                {
                    var newTeacher = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                        predicate: t => t.Id == request.UpdateCourseDTO.TeacherId
                    );

                    if (newTeacher == null)
                    {
                        _logger.LogWarning($"Teacher with ID {request.UpdateCourseDTO.TeacherId} not found.");
                        return Result.Fail($"Teacher with ID {request.UpdateCourseDTO.TeacherId} not found.");
                    }
                }

                // Обновляем только изменённые поля
                existingCourse.Title = request.UpdateCourseDTO.Title;
                existingCourse.Description = request.UpdateCourseDTO.Description;
                existingCourse.Credits = request.UpdateCourseDTO.Credits;
                existingCourse.TeacherId = request.UpdateCourseDTO.TeacherId;

                _repositoryWrapper.CourseRepository.Update(existingCourse);
                await _repositoryWrapper.SaveChangesAsync();

                var updatedCourseDTO = _mapper.Map<CourseDTO>(existingCourse);

                _logger.LogInformation($"Course with ID {existingCourse.Id} successfully updated.");
                return Result.Ok(updatedCourseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating course ID {request.UpdateCourseDTO.Id}");
                return Result.Fail("An error occurred while updating the course.");
            }
        }
    }
}
