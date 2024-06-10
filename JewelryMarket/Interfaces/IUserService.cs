using JewelryMarket.Entities;
using JewelryMarket.Models;

namespace JewelryMarket.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<int> AddUserAsync(UserDto request);
        Task UpdateUserAsync(User user, UpdateUserDto request);
        Task DeleteUserAsync(User user);
        Task<string> LoginAsync(LoginModel request);
        Task UpdateUserRoleAsync(User user);
        Task UpdateUserBalanceAsync(User user);
        Task ChangePasswordAsync(User user, string newPassword);
        Task<string> CreateToken(User user); 
        Task<string> CreateRefreshToken(User user);
        Task<bool> ValidateRefreshToken(User user, string refreshToken);
        Task RevokeRefreshToken(User user);
        Task<bool> VerifyPasswordAsync(User user, string password);
    }
}
