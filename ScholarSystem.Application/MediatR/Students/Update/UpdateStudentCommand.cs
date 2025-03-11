using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.MediatR.Students.Update
{
    public record UpdateStudentCommand(UpdateStudentDTO UpdateStudentDTO) : IRequest<Result<UpdateStudentDTO>>;

}
