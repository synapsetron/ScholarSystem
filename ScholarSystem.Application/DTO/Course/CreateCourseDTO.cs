
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Course
{
    public  class CreateCourseDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Credits { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}
