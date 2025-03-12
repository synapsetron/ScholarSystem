using FluentValidation;
using ScholarSystem.Application.MediatR.Courses.GetAll;

namespace ScholarSystem.Application.Validation.Course
{
    public class GetAllCoursesQueryValidator : AbstractValidator<GetAllCoursesQuery>
    {
        public GetAllCoursesQueryValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("Query cannot be null.");
        }
    }
}
