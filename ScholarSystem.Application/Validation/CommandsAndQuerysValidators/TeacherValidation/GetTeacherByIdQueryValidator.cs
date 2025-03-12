using FluentValidation;
using ScholarSystem.Application.MediatR.Teachers.GetByTeacherId;

namespace ScholarSystem.Application.Validation.Teacher
{
    public class GetTeacherByIdQueryValidator : AbstractValidator<GetTeacherByIdQuery>
    {
        public GetTeacherByIdQueryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Teacher ID must be greater than zero.");
        }
    }
}
