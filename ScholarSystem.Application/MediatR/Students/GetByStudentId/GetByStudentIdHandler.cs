using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScholarSystem.Application.MediatR.Students.GetByStudentId
{
    public class GetByStudentIdHandler : IRequestHandler<GetByStudentIdQuery, Result<StudentDTO>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetByStudentIdHandler> _logger;

        public GetByStudentIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<GetByStudentIdHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<StudentDTO>> Handle(GetByStudentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // find student by ID and include courses
                var student = await _repositoryWrapper.StudentRepository.GetFirstOrDefaultAsync(
                    predicate: s => s.Id == request.Id,
                    include: query => query.Include(s => s.StudentCourses).ThenInclude(sc => sc.Course));

                if (student == null)
                {
                    _logger.LogWarning($"Student with ID {request.Id} not found.");
                    return Result.Fail($"Student with ID {request.Id} not found.");
                }

                var studentDTO = _mapper.Map<StudentDTO>(student);

                _logger.LogInformation($"Successfully retrieved student: {studentDTO.Name}");
                return Result.Ok(studentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching student ID {request.Id}");
                return Result.Fail("An error occurred while retrieving the student.");
            }
        }
    }
}
