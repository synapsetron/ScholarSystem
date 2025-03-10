using AutoMapper;
using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScholarSystem.Application.MediatR.Courses.Create
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<CreateCourseDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCourseHandler> _logger;

        public CreateCourseHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<CreateCourseHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<CreateCourseDTO>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var teacherExists = await _repositoryWrapper.TeacherRepository
                    .GetFirstOrDefaultAsync(t => t.Id == request.courseDTO.TeacherId);

                if (teacherExists == null)
                {
                    _logger.LogWarning($"Teacher with ID {request.courseDTO.TeacherId} not found.");
                    return Result.Fail($"Teacher with ID {request.courseDTO.TeacherId} not found.");
                }

                // Создаем курс
                var course = _mapper.Map<Course>(request.courseDTO);
                var createdCourse = _repositoryWrapper.CourseRepository.Create(course);


                await _repositoryWrapper.SaveChangesAsync();

                var createdCourseDTO = _mapper.Map<CreateCourseDTO>(createdCourse);

                _logger.LogInformation($"Course '{createdCourse.Title}' created successfully.");
                return Result.Ok(createdCourseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the course.");
                return Result.Fail("An error occurred while creating the course.");
            }
        }
    }
}
