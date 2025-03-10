using FluentResults;
using ScholarSystem.Application.DTO.Course;
using MediatR;

namespace ScholarSystem.Application.MediatR.Courses.Create
{
    public record CreateCourseCommand (CreateCourseDTO courseDTO): IRequest<Result<CreateCourseDTO>>;
}
