using FluentValidation;
using ScholarSystem.Application.DTO.Teacher;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class DeleteTeacherDTOValidator : AbstractValidator<DeleteTeacherDTO>
    {
        public DeleteTeacherDTOValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0).WithMessage("Teacher ID must be greater than zero.");
        }
    }
}
