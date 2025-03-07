
namespace EntryTask.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
