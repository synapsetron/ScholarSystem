using System.ComponentModel.DataAnnotations;

namespace ScholarSystem.Application.DTO.Teacher
{
    public class UpdateTeacherDTO: CreateTeacherDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
