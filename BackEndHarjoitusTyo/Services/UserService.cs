using BackEndHarjoitusTyo.Middleware;
using BackEndHarjoitusTyo.Models;
using BackEndHarjoitusTyo.Repositories;
using Humanizer;

namespace BackEndHarjoitusTyo.Services
{
    public class UserService : iUserService
    {
        private readonly iUserRepository _repository;
        private readonly iUserAuthenticationService _userauthenticationService;

        public UserService(iUserRepository repository, iUserAuthenticationService userAuthenticationService)
        {
            _repository = repository;
            _userauthenticationService = userAuthenticationService;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            User? user = await _repository.GetUserAsync(username);
            if (user == null)
            {
                return await _repository.DeleteUserAsync(user);
            }
            return false;
        }

        public async Task<UserDTO> GetUserAsync(string username)
        {
            User? user = await _repository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }
            return UserToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            IEnumerable<User> users = await _repository.GetUsersAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (User user in users)
            {
                result.Add(UserToDTO(user));
            }
            return result;
        }

        public async Task<UserDTO> NewUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.UserName);
            if (dbUser != null)
            {
                return null;
            }
            user.JoinDate = DateTime.Now;
            user.LastLogin = DateTime.Now;

            User? newUser = _userauthenticationService.CreateUserCredentials(user);
            if (newUser != null)
            {
                return UserToDTO(await _repository.NewUserAsync(user));
            }
            return null;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.UserName);
            if (dbUser != null)
            {
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Email = user.Email;
                dbUser.Password = user.Password;
                return await _repository.UpdateUserAsync(dbUser);
            }
            return false;
        }

        private UserDTO UserToDTO(User user)
        {
            UserDTO dto = new UserDTO();
            dto.UserName = user.UserName;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;  
            dto.Email = user.Email;
            dto.JoinDate = user.JoinDate;
            dto.LastLogin = user.LastLogin;

            return dto;
        }
    }
}
