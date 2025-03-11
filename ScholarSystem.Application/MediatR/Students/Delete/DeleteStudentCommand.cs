using FluentResults;
using MediatR;

namespace ScholarSystem.Application.MediatR.Students.Delete
{
    public record class DeleteStudentCommand ( int id) : IRequest<Result<Unit>>;
}
