using FluentResults;
using MediatR;


namespace ScholarSystem.Application.MediatR.Teachers.Delete
{
    public record DeleteTeacherCommand (int Id) : IRequest<Result<Unit>>;

}
