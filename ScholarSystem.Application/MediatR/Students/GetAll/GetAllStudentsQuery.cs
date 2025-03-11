using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.MediatR.Students.GetAll
{
    public record GetAllStudentsQuery() : IRequest<Result<IEnumerable<StudentDTO>>>;
}
