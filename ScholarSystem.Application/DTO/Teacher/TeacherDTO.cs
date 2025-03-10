
namespace ScholarSystem.Application.DTO.Teacher
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> CourseTitles { get; set; } = new();
    }
}
