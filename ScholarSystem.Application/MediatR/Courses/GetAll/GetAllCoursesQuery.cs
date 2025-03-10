using ScholarSystem.Application.DTO.Course;
using FluentResults;
using MediatR;
namespace ScholarSystem.Application.MediatR.Courses.GetAll
{
    public record GetAllCoursesQuery(): IRequest<Result<IEnumerable<CourseDTO>>>;

}
