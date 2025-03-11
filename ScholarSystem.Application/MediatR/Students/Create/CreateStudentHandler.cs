using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Domain.Entities;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScholarSystem.Application.MediatR.Students.Create
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<CreateStudentDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateStudentHandler> _logger;

        public CreateStudentHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<CreateStudentHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<CreateStudentDTO>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // check if student with this email exists or no
                var existingStudent = await _repositoryWrapper.StudentRepository
                    .GetFirstOrDefaultAsync(s => s.Email == request.studentDTO.Email);

                if (existingStudent != null)
                {
                    _logger.LogWarning($"Student with email {request.studentDTO.Email} already exists.");
                    return Result.Fail($"Student with email {request.studentDTO.Email} already exists.");
                }

                // сreate a new student
                var student = _mapper.Map<Student>(request.studentDTO);

                var createdStudent = _repositoryWrapper.StudentRepository.Create(student);
                await _repositoryWrapper.SaveChangesAsync();

                var createdStudentDTO = _mapper.Map<CreateStudentDTO>(createdStudent);

                _logger.LogInformation($"Student '{createdStudent.Name}' created successfully.");
                return Result.Ok(createdStudentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the student.");
                return Result.Fail("An error occurred while creating the student.");
            }
        }
    }
}
