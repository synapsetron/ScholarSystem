using FluentValidation;
using ScholarSystem.Application.DTO.Course;

namespace ScholarSystem.Application.Validation.DTOValidators.Сourse
{
    public class UpdateCourseDTOValidator : AbstractValidator<UpdateCourseDTO>
    {
        public UpdateCourseDTOValidator()
        {
            Include(new CreateCourseDTOValidator());

            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Course ID must be greater than zero.");
        }
    }
}
