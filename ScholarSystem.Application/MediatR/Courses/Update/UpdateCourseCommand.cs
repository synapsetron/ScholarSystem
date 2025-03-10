using ScholarSystem.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace ScholarSystem.Application.MediatR.Courses.Update
{
    public record UpdateCourseCommand(UpdateCourseDTO UpdateCourseDTO) : IRequest<Result<CourseDTO>>;
}
