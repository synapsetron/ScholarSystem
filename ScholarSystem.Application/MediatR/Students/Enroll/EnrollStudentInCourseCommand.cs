using FluentResults;
using MediatR;
using ScholarSystem.Application.DTO.Student;

namespace ScholarSystem.Application.MediatR.Students.Courses
{
    public record EnrollStudentInCourseCommand(EnrollStudentInCourseDTO EnrollStudentDTO) : IRequest<Result<Unit>>;
}
