using ScholarSystem.Application.DTO.User;

namespace ScholarSystem.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<UserResponseDTO> RegisterAsync(UserRegisterDTO request);
        Task<CurrentUserResponseDTO> GetCurrentUserAsync();
        Task<UserResponseDTO> GetByIdAsync(Guid id);
        Task<UserResponseDTO> UpdateAsync(Guid id, UpdateUserRequestDTO request);
        Task DeleteAsync(Guid id);
        Task<RevokeRefreshTokenResponseDTO> RevokeRefreshToken(RefreshTokenRequestDTO refreshTokenRemoveRequest);
        Task<CurrentUserResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request);

        Task<UserResponseDTO> LoginAsync(UserLoginDTO request);
    }
}
