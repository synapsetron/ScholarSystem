using EntryTask.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.Update
{
    public record UpdateCourseCommand(CourseDTO CourseDto) : IRequest<Result<CourseDTO>>;
}
