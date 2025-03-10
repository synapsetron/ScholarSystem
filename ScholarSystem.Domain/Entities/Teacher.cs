using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Domain.Entities
{

    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        // Навигационное свойство: у преподавателя могут быть курсы
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
