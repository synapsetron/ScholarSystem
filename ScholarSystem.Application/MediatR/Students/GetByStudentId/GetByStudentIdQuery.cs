using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Student;


namespace ScholarSystem.Application.MediatR.Students.GetByStudentId
{
    public record GetByStudentIdQuery(int Id) : IRequest<Result<StudentDTO>>;
}
