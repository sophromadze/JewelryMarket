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
        //Task UpdateUserAsync(User user, UserDto request);
        Task UpdateUserAsync(User user, UpdateUserDto request);
        Task DeleteUserAsync(User user);
        Task<string> LoginAsync(LoginModel request);
        Task UpdateUserRoleAsync(User user);
        Task UpdateUserBalanceAsync(User user);
        Task ChangePasswordAsync(User user, string newPassword);
        Task<bool> VerifyPasswordAsync(User user, string password);
    }
}
