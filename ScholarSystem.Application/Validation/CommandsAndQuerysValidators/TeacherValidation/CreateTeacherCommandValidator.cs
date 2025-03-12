using FluentValidation;
using ScholarSystem.Application.MediatR.Teachers.Create;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
    {
        public CreateTeacherCommandValidator()
        {
            RuleFor(x => x.teacherDTO)
                .NotNull().WithMessage("TeacherDTO cannot be null.")
                .SetValidator(new CreateTeacherDTOValidator()); 
        }
    }
}
