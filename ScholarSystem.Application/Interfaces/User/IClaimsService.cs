using ScholarSystem.Domain.Entities;
using System.Security.Claims;

namespace ScholarSystem.Application.Interfaces.User
{
    public interface IClaimsService
    {
        Task<List<Claim>> CreateClaimsAsync(ApplicationUser user);
    }
}
