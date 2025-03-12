using FluentValidation;
using ScholarSystem.Application.MediatR.Teachers.Update;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherCommandValidator()
        {
            RuleFor(x => x.updateTeacherDTO)
                .NotNull().WithMessage("UpdateTeacherDTO cannot be null.")
                .SetValidator(new UpdateTeacherDTOValidator());
        }
    }
}
