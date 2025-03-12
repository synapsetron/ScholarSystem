using FluentValidation;
using ScholarSystem.Application.MediatR.Students.GetByStudentId;

namespace ScholarSystem.Application.Validation.Student
{
    public class GetByStudentIdQueryValidator : AbstractValidator<GetByStudentIdQuery>
    {
        public GetByStudentIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Student ID must be greater than zero.");
        }
    }
}
