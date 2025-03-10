namespace ScholarSystem.Application.DTO.Course
{
    public class CourseDTO
    {
        public int Id { get; init; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
    }
}
