using FluentValidation;
using ScholarSystem.Application.MediatR.Teachers.Delete;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
    {
        public DeleteTeacherCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Teacher ID must be greater than zero.");
        }
    }
}
