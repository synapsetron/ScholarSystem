using FluentResults;
using EntryTask.Application.DTO.Course;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.Create
{
    public record CreateCourseCommand (CreateCourseDTO courseDTO): IRequest<Result<CreateCourseDTO>>;
}
