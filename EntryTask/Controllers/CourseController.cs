using EntryTask.Application.DTO.Course;
using EntryTask.Application.MediatR.Courses.Create;
using EntryTask.Application.MediatR.Courses.Delete;
using EntryTask.Application.MediatR.Courses.GetAll;
using EntryTask.Application.MediatR.Courses.GetByCourseId;
using EntryTask.Application.MediatR.Courses.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.WebApi.Controllers;

namespace EntryTask.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return HandleResult(await Mediator.Send(new GetAllCoursesQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await Mediator.Send(new GetCoursesByIdQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO courseDto)
        {    
            return HandleResult(await Mediator.Send(new CreateCourseCommand(courseDto)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CourseDTO courseDto)
        {
            return HandleResult(await Mediator.Send(new UpdateCourseCommand(courseDto)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteCourseCommand(id)));
            
        }
    }
} 