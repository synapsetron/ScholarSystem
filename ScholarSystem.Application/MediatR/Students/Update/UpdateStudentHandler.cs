using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;


namespace ScholarSystem.Application.MediatR.Students.Update
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Result<UpdateStudentDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateStudentHandler> _logger;

        public UpdateStudentHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<UpdateStudentHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<UpdateStudentDTO>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // check is student exists
                var existingStudent = await _repositoryWrapper.StudentRepository.GetFirstOrDefaultAsync(
                    predicate: s => s.Id == request.UpdateStudentDTO.Id);

                if (existingStudent == null)
                {
                    _logger.LogWarning($"Student with ID {request.UpdateStudentDTO.Id} not found.");
                    return Result.Fail($"Student with ID {request.UpdateStudentDTO.Id} not found.");
                }

                // update properties
                existingStudent.Name = request.UpdateStudentDTO.Name;
                existingStudent.Email = request.UpdateStudentDTO.Email;
                existingStudent.EnrollmentDate = request.UpdateStudentDTO.EnrollmentDate;

                _repositoryWrapper.StudentRepository.Update(existingStudent);
                await _repositoryWrapper.SaveChangesAsync();

                var updatedStudentDTO = _mapper.Map<UpdateStudentDTO>(existingStudent);

                _logger.LogInformation($"Student with ID {existingStudent.Id} successfully updated.");
                return Result.Ok(updatedStudentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating student ID {request.UpdateStudentDTO.Id}");
                return Result.Fail("An error occurred while updating the student.");
            }
        }
    }
}
