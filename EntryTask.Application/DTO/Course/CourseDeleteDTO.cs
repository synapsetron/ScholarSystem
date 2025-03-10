
using System.ComponentModel.DataAnnotations;

namespace EntryTask.Application.DTO.Course
{
    public class CourseDeleteDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
