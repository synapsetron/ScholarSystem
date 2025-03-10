using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;

namespace ScholarSystem.Application.MediatR.Teachers.Update
{
    public class UpdateTeacherHandler : IRequestHandler<UpdateTeacherCommand, Result<TeacherDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTeacherHandler> _logger;

        public UpdateTeacherHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<UpdateTeacherHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<TeacherDTO>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // check if teacher exists
                var existingTeacher = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                    t => t.Id == request.updateTeacherDTO.Id
                );

                if (existingTeacher == null)
                {
                    _logger.LogWarning($"Teacher with ID {request.updateTeacherDTO.Id} not found.");
                    return Result.Fail($"Teacher with ID {request.updateTeacherDTO.Id} not found.");
                }

                // check if another teacher with the same email exists
                var teacherWithSameEmail = await _repositoryWrapper.TeacherRepository.GetFirstOrDefaultAsync(
                    t => t.Email == request.updateTeacherDTO.Email && t.Id != request.updateTeacherDTO.Id
                );

                if (teacherWithSameEmail != null)
                {
                    _logger.LogWarning($"Another teacher with email {request.updateTeacherDTO.Email} already exists.");
                    return Result.Fail($"Another teacher with email {request.updateTeacherDTO.Email} already exists.");
                }

                // Update information about teacher
                existingTeacher.Name = request.updateTeacherDTO.Name;
                existingTeacher.Email = request.updateTeacherDTO.Email;

                _repositoryWrapper.TeacherRepository.Update(existingTeacher);
                await _repositoryWrapper.SaveChangesAsync();

                var updatedTeacherDTO = _mapper.Map<TeacherDTO>(existingTeacher);

                _logger.LogInformation($"Teacher with ID {existingTeacher.Id} successfully updated.");
                return Result.Ok(updatedTeacherDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating teacher ID {request.updateTeacherDTO.Id}");
                return Result.Fail("An error occurred while updating the teacher.");
            }
        }
    }
}
