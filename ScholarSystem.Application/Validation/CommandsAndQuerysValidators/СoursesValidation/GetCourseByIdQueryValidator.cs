using FluentValidation;
using ScholarSystem.Application.MediatR.Courses.GetByCourseId;

namespace ScholarSystem.Application.Validation.Course
{
    public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
    {
        public GetCourseByIdQueryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Course ID must be greater than zero.");
        }
    }
}
