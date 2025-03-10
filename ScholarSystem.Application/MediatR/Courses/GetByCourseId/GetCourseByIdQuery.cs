using ScholarSystem.Application.DTO.Course;
using FluentResults;
using MediatR;

namespace ScholarSystem.Application.MediatR.Courses.GetByCourseId
{
    public record GetCourseByIdQuery(int id) : IRequest<Result<CourseDTO>>;

}
