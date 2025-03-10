
using FluentResults;
using MediatR;

namespace ScholarSystem.Application.MediatR.Courses.Delete
{
    public record DeleteCourseCommand(int id) : IRequest<Result<Unit>>;
}
