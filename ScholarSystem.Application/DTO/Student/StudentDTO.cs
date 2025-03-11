namespace ScholarSystem.Application.DTO.Student
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<string> CourseTitles { get; set; } = new();
    }
}
