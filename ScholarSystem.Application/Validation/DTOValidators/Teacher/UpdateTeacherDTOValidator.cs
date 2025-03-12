using FluentValidation;
using ScholarSystem.Application.DTO.Teacher;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class UpdateTeacherDTOValidator : AbstractValidator<UpdateTeacherDTO>
    {
        public UpdateTeacherDTOValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0).WithMessage("Teacher ID must be greater than zero.");

            Include(new CreateTeacherDTOValidator());
        }
    }
}
