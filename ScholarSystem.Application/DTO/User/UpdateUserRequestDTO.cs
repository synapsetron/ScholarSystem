﻿
namespace ScholarSystem.Application.DTO.User
{
    public class UpdateUserRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
    }
}
