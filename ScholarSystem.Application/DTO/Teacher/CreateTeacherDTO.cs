using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Teacher
{
    public class CreateTeacherDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
