using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.MediatR.Students.Create
{
    public record CreateStudentCommand(CreateStudentDTO studentDTO) : IRequest<Result<CreateStudentDTO>>;
}
