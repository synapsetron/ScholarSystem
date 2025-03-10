using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;

namespace ScholarSystem.Application.MediatR.Teachers.GetByTeacherId
{
    public record GetTeacherByIdQuery(int id) : IRequest<Result<TeacherDTO>>;
}
