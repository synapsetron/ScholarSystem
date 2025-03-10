using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Teacher;


namespace ScholarSystem.Application.MediatR.Teachers.GetAll
{
    public record GetAllTeachersQuery(): IRequest<Result<IEnumerable<TeacherDTO>>>;
}
