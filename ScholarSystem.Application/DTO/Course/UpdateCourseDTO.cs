using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Course
{
    public class UpdateCourseDTO : CreateCourseDTO
    {
        [Required]
        public int Id { get; init; }
    }
}
