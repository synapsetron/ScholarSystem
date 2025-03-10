using System.ComponentModel.DataAnnotations;


namespace EntryTask.Domain.Entities
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}
