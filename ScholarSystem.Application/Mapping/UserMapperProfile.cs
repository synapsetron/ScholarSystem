using AutoMapper;
using ScholarSystem.Application.DTO.User;
using ScholarSystem.Domain.Entities;


namespace ScholarSystem.Application.Mapping
{
    public class UserMapperProfile : Profile
    {
      public UserMapperProfile()
      {
         CreateMap<ApplicationUser, UserResponseDTO>();
         CreateMap<ApplicationUser, CurrentUserResponseDTO>();
         CreateMap<UserRegisterDTO, ApplicationUser>();
      }
    }
}
