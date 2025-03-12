using FluentValidation;
using ScholarSystem.Application.DTO.Course;

namespace ScholarSystem.Application.Validation.DTOValidators.Сourse
{
    public class CourseDeleteDTOValidator : AbstractValidator<CourseDeleteDTO>
    {
        public CourseDeleteDTOValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Course ID must be greater than zero.");
        }
    }
}
