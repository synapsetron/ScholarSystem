using AutoMapper;
using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ScholarSystem.Application.MediatR.Teachers.GetByTeacherId
{
    public class GetTeacherByIdHandler : IRequestHandler<GetTeacherByIdQuery, Result<TeacherDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTeacherByIdHandler> _logger;

        public GetTeacherByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<GetTeacherByIdHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<TeacherDTO>> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // get a teacher with his courses
                var teacher = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                    predicate: t => t.Id == request.id,
                    include: query => query.Include(t => t.Courses)
                );

                if (teacher == null)
                {
                    _logger.LogWarning($"Teacher with ID {request.id} not found.");
                    return Result.Fail($"Teacher with ID {request.id} not found.");
                }

                var teacherDTO = _mapper.Map<TeacherDTO>(teacher);

                _logger.LogInformation($"Successfully retrieved teacher: {teacherDTO.Name}");
                return Result.Ok(teacherDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching teacher ID {request.id}");
                return Result.Fail("An error occurred while retrieving the teacher.");
            }
        }
    }
}
