using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;

namespace ScholarSystem.Application.MediatR.Teachers.Create
{
    public class CreateTeacherHandler : IRequestHandler<CreateTeacherCommand, Result<CreateTeacherDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTeacherHandler> _logger;

        public CreateTeacherHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<CreateTeacherHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<CreateTeacherDTO>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // сheck if teacher with the same email already exists
                var existingTeacher = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                    t => t.Email == request.teacherDTO.Email
                );

                if (existingTeacher != null)
                {
                    _logger.LogWarning($"Teacher with email {request.teacherDTO.Email} already exists.");
                    return Result.Fail($"Teacher with email {request.teacherDTO.Email} already exists.");
                }

                // Create a new teacher
                var teacher = _mapper.Map<Teacher>(request.teacherDTO);
                var createdTeacher = _repositoryWrapper.TeacherRepository.Create(teacher);

                await _repositoryWrapper.SaveChangesAsync();

                var createdTeacherDTO = _mapper.Map<CreateTeacherDTO>(createdTeacher);

                _logger.LogInformation($"Teacher '{createdTeacher.Name}' created successfully.");
                return Result.Ok(createdTeacherDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the teacher.");
                return Result.Fail("An error occurred while creating the teacher.");
            }
        }
    }
}
