
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Course
{
    public class CourseDeleteDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
