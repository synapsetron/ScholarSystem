using EntryTask.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.GetByCourseId
{
    public record GetCoursesByIdQuery(int id) : IRequest<Result<CourseDTO>>;

}
