
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Course
{
    public  class CreateCourseDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
        public int Credits { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}
