using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntryTask.Domain.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int Credits { get; set; }

        // Связь 1:M - У курса есть один преподаватель
        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Связь M:N - Один курс могут проходить много студентов
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
