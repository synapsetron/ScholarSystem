
using Microsoft.AspNetCore.Mvc;
using ScholarSystem.Application.DTO.Student;
using ScholarSystem.Application.MediatR.Students.Create;
using ScholarSystem.Application.MediatR.Students.Delete;
using ScholarSystem.Application.MediatR.Students.GetAll;
using ScholarSystem.Application.MediatR.Students.Update;

using ScholarSystem.WebApi.Controllers;
using ScholarSystem.Application.MediatR.Students.GetByStudentId;
using ScholarSystem.Application.MediatR.Students.Courses;

namespace ScholarSystem.WebAPI.Controllers
{
    public class StudentController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {

            return HandleResult(await Mediator.Send(new GetAllStudentsQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetByStudentIdQuery(id), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO teacherDTO, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new CreateStudentCommand(teacherDTO), cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStudentDTO teacherDTO, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new UpdateStudentCommand(teacherDTO), cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new DeleteStudentCommand(id), cancellationToken));

        }

        [HttpPost("{studentId}/courses/{courseId}")]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId, CancellationToken cancellationToken)
        {
            var command = new EnrollStudentInCourseCommand(new EnrollStudentInCourseDTO { StudentId = studentId, CourseId = courseId });
            return HandleResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{studentId}/courses/{courseId}")]
        public async Task<IActionResult> UnenrollStudent(int studentId, int courseId, CancellationToken cancellationToken)
        {
            var command = new UnenrollStudentFromCourseCommand(new UnenrollStudentFromCourseDTO { StudentId = studentId, CourseId = courseId });
            return HandleResult(await Mediator.Send(command, cancellationToken));
        }

    }
}
