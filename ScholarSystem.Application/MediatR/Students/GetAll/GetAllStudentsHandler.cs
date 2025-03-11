using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ScholarSystem.Application.MediatR.Students.GetAll
{
    public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, Result<IEnumerable<StudentDTO>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllStudentsHandler> _logger;

        public GetAllStudentsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILogger<GetAllStudentsHandler> logger)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<IEnumerable<StudentDTO>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // get all students with their courses
                var students = await _repositoryWrapper.StudentRepository.GetAllAsync(
                    include: query => query.Include(s => s.StudentCourses).ThenInclude(sc => sc.Course));

                if (students == null || !students.Any())
                {
                    _logger.LogWarning("No students found in the database.");
                    return Result.Ok(Enumerable.Empty<StudentDTO>());
                }

                var studentDTOs = _mapper.Map<IEnumerable<StudentDTO>>(students);

                _logger.LogInformation($"Successfully retrieved {studentDTOs.Count()} students.");
                return Result.Ok(studentDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching students.");
                return Result.Fail("An error occurred while retrieving students.");
            }
        }
    }
}
