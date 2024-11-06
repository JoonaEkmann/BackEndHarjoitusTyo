using BackEndHarjoitusTyo.Models;
namespace BackEndHarjoitusTyo.Services
{
    public interface iUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserAsync(string username);
        Task<UserDTO> NewUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string username);
    }
}
