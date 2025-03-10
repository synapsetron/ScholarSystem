using ScholarSystem.Application.DTO.Teacher;
using ScholarSystem.Application.MediatR.Teachers.Create;
using ScholarSystem.Application.MediatR.Teachers.Delete;
using ScholarSystem.Application.MediatR.Teachers.GetAll;
using ScholarSystem.Application.MediatR.Teachers.GetByTeacherId;
using ScholarSystem.Application.MediatR.Teachers.Update;
using Microsoft.AspNetCore.Mvc;
using ScholarSystem.WebApi.Controllers;


namespace ScholarSystem.WebAPI.Controllers
{
    public class TeacherController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {

            return HandleResult(await Mediator.Send(new GetAllTeachersQuery(), cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetTeacherByIdQuery(id), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherDTO teacherDTO, CancellationToken cancellationToken)
        {    
            return HandleResult(await Mediator.Send(new CreateTeacherCommand(teacherDTO), cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTeacherDTO teacherDTO, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new UpdateTeacherCommand(teacherDTO), cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new DeleteTeacherCommand(id), cancellationToken));
            
        }
    }
} 