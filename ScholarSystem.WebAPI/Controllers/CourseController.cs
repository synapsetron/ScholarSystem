using ScholarSystem.Application.DTO.Course;
using ScholarSystem.Application.MediatR.Courses.Create;
using ScholarSystem.Application.MediatR.Courses.Delete;
using ScholarSystem.Application.MediatR.Courses.GetAll;
using ScholarSystem.Application.MediatR.Courses.GetByCourseId;
using ScholarSystem.Application.MediatR.Courses.Update;
using Microsoft.AspNetCore.Mvc;
using ScholarSystem.WebApi.Controllers;


namespace ScholarSystem.WebAPI.Controllers
{
    public class CourseController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {

            return HandleResult(await Mediator.Send(new GetAllCoursesQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetCourseByIdQuery(id), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO courseDto, CancellationToken cancellationToken)
        {    
            return HandleResult(await Mediator.Send(new CreateCourseCommand(courseDto), cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDTO courseDto, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new UpdateCourseCommand(courseDto), cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new DeleteCourseCommand(id), cancellationToken));
            
        }
    }
} 