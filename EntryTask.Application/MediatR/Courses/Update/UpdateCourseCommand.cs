using EntryTask.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace EntryTask.Application.MediatR.Courses.Update
{
    public record UpdateCourseCommand(UpdateCourseDTO UpdateCourseDTO) : IRequest<Result<CourseDTO>>;
}
