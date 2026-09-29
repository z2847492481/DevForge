using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DevForge.Api.Data;
using DevForge.Api.Dtos;
using DevForge.Api.Entities;
using DevForge.Api.Security;
using DevForge.Api.Services;

namespace DevForge.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly PasswordHasher _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AppDbContext db,
        ITokenService tokenService,
        PasswordHasher passwordHasher,
        ILogger<AuthController> logger)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var usernameExists = await _db.Users.AnyAsync(u => u.Username == request.Username);
        if (usernameExists)
        {
            return Conflict(new ErrorResponse { Message = "Username is already taken." });
        }

        var emailExists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (emailExists)
        {
            return Conflict(new ErrorResponse { Message = "Email is already in use." });
        }

        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            FullName = request.FullName,
            Role = "User",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Registered new user {Username} ({UserId})", user.Username, user.Id);

        var (token, expiresAt) = _tokenService.CreateToken(user);
        return Ok(ToAuthResponse(user, token, expiresAt));
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var key = request.UsernameOrEmail.Trim();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == key || u.Email == key);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            // Same message for unknown user vs wrong password — do not leak accounts.
            return Unauthorized(new ErrorResponse { Message = "Invalid username/email or password." });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new ErrorResponse { Message = "This account is deactivated." });
        }

        _logger.LogInformation("User {Username} ({UserId}) signed in", user.Username, user.Id);

        var (token, expiresAt) = _tokenService.CreateToken(user);
        return Ok(ToAuthResponse(user, token, expiresAt));
    }

    private static AuthResponse ToAuthResponse(User user, string token, long expiresAt) => new()
    {
        Token = token,
        ExpiresAt = expiresAt,
        User = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        }
    };
}