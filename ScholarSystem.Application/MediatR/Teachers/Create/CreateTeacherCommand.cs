using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;

namespace ScholarSystem.Application.MediatR.Teachers.Create
{
    public record CreateTeacherCommand(CreateTeacherDTO teacherDTO): IRequest<Result<CreateTeacherDTO>>;
}