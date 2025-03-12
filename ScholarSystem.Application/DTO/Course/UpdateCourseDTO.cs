using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Course
{
    public class UpdateCourseDTO : CreateCourseDTO
    {
        public int Id { get; init; }
    }
}
