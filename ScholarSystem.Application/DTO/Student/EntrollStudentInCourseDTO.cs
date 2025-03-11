
using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Student
{
    public class EnrollStudentInCourseDTO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}

