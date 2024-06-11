using JewelryMarket.Entities;
using JewelryMarket.Interfaces;
using JewelryMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JewelryMarket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(UserDto request)
    {
        var result = await _userService.AddUserAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login(LoginModel request)
    {
        var user = await _userService.GetUserByUsernameOrEmailAsync(request.UserName);
        if (user == null)
        {
            return Unauthorized("User not found");
        }

        var token = await _userService.LoginAsync(request);
        if (token == "User not found" || token == "Wrong Password!")
        {
            return Unauthorized(token);
        }

        var refreshToken = await _userService.CreateRefreshToken(user);
        return Ok(new { token, refreshToken, username = user.UserName, role = user.Role.ToString() });
    }

    [HttpGet("username/{username}"), Authorize]
    public async Task<ActionResult<User>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameOrEmailAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("change-password"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        if (request == null || string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
        {
            return BadRequest("Invalid request data");
        }

        var username = User.Identity.Name;
        var user = await _userService.GetUserByUsernameOrEmailAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        var isPasswordValid = await _userService.VerifyPasswordAsync(user, request.CurrentPassword);
        if (!isPasswordValid)
        {
            return BadRequest(new { message = "Current password is incorrect" });
        }

        await _userService.ChangePasswordAsync(user, request.NewPassword);

        return NoContent();
    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto request)
    {
        //check if user exists 
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _userService.UpdateUserAsync(user, request);

        return NoContent();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _userService.DeleteUserAsync(user);

        return NoContent();
    }

    [HttpGet("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPatch("{id}/role"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(int id, UserRoleDto request)
    {
        // Check if user exists
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Update role
        user.Role = request.Role;
        await _userService.UpdateUserRoleAsync(user);

        return NoContent();
    }

    [HttpPatch("{id}/balance"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserBalance(int id, BalanceDto request)
    {
        // Check if user exists
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Update balance
        user.Balance = request.Balance;
        await _userService.UpdateUserBalanceAsync(user);

        return NoContent();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<object>> RefreshToken([FromBody] RefreshTokenModel request)
    {
        var user = await _userService.GetUserByUsernameOrEmailAsync(request.Username);
        if (user == null)
        {
            return Unauthorized("Invalid refresh token");
        }

        var isValid = await _userService.ValidateRefreshToken(user, request.RefreshToken);
        if (!isValid)
        {
            return Unauthorized("Invalid or expired refresh token");
        }

        var newToken = await _userService.CreateToken(user);
        var newRefreshToken = await _userService.CreateRefreshToken(user);

        return Ok(new { token = newToken, refreshToken = newRefreshToken });
    }

    [HttpPost("revoke-token"), Authorize]
    public async Task<IActionResult> RevokeToken()
    {
        var username = User.Identity.Name;
        var user = await _userService.GetUserByUsernameOrEmailAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        await _userService.RevokeRefreshToken(user);
        return NoContent();
    }
}
