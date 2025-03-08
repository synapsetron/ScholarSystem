using EntryTask.Application.DTO.Course;
using FluentResults;
using MediatR;
namespace EntryTask.Application.MediatR.Courses.GetAll
{
    public record GetAllCoursesQuery(): IRequest<Result<IEnumerable<CourseDTO>>>;

}
