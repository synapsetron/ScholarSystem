using FluentValidation;
using ScholarSystem.Application.MediatR.Students.Update;

namespace ScholarSystem.Application.Validation.Student
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.UpdateStudentDTO)
                .NotNull().WithMessage("UpdateStudentDTO cannot be null.")
                .SetValidator(new UpdateStudentDTOValidator());
        }
    }
}
