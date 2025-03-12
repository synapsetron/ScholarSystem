using ScholarSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScholarSystem.Application.Interfaces.User
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
