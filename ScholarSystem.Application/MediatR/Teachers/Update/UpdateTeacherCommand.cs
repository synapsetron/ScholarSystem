using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;


namespace ScholarSystem.Application.MediatR.Teachers.Update
{
    public record UpdateTeacherCommand(UpdateTeacherDTO updateTeacherDTO) : IRequest<Result<TeacherDTO>>;
}
