
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.Delete
{
    public record DeleteCourseCommand(int id) : IRequest<Result<Unit>>;
}
