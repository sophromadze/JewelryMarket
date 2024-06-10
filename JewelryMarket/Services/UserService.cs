using JewelryMarket.Entities;
using JewelryMarket.Interfaces;
using JewelryMarket.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JewelryMarket.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
    {
        return await _userRepository.GetUserByUsernameOrEmailAsync(usernameOrEmail);
    }

    public async Task<string> LoginAsync(LoginModel request)
    {
        var user = await GetUserByUsernameOrEmailAsync(request.UserName);

        if (user == null)
        {
            return "User not found";
        }
        else if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return "Wrong Password!";
        }

        return await CreateToken(user);
    }

    public async Task<int> AddUserAsync(UserDto request)
    {
        CreateHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            UserName = request.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            PersonalId = request.PersonalId,
            PhoneNumber = request.PhoneNumber,
        };

        await _userRepository.AddUserAsync(user);

        var userId = user.Id;

        return userId;
    }

    public async Task UpdateUserAsync(User user, UpdateUserDto request)
{
    if (!string.IsNullOrEmpty(request.Password))
    {
        CreateHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
    }

    user.UserName = request.UserName;
    user.Email = request.Email;
    user.Address = request.Address;
    user.PhoneNumber = request.PhoneNumber;
    user.PersonalId = request.PersonalId;
    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.UpdatedAt = DateTime.UtcNow;

    await _userRepository.UpdateUserAsync(user);
}

    public async Task DeleteUserAsync(User user)
    {
        await _userRepository.DeleteUserAsync(user);
    }

    public async Task UpdateUserRoleAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task UpdateUserBalanceAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user); 
    }

    public async Task ChangePasswordAsync(User user, string newPassword)
    {
        CreateHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> VerifyPasswordAsync(User user, string password)
    {
        return VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
    }

    private void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordSalt = hmac.Key;
        }
    }

    public async Task<string> CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new (ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    public async Task<string> CreateRefreshToken(User user)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.TokenCreatedAt = DateTime.Now;
        user.TokenUpdatedAt = DateTime.Now.AddMinutes(10); // Refresh token valid for 1 minute

        await _userRepository.UpdateUserAsync(user);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshToken(User user, string refreshToken)
    {
        if (user.RefreshToken != refreshToken || user.TokenUpdatedAt < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public async Task RevokeRefreshToken(User user)
    {
        user.RefreshToken = null;
        user.TokenCreatedAt = null;
        user.TokenUpdatedAt = null;
        await _userRepository.UpdateUserAsync(user);
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(passwordHash);
        }
    }
}
