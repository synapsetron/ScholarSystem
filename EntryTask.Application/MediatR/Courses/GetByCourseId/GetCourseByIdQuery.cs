using EntryTask.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.GetByCourseId
{
    public record GetCourseByIdQuery(int id) : IRequest<Result<CourseDTO>>;

}
