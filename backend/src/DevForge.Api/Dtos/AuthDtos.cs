using System.ComponentModel.DataAnnotations;

namespace DevForge.Api.Dtos;

public record RegisterRequest
{
    [Required, MinLength(2), MaxLength(255)]
    public string Username { get; init; } = null!;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; init; } = null!;

    [Required, MinLength(6), MaxLength(128)]
    public string Password { get; init; } = null!;

    [MaxLength(255)]
    public string? FullName { get; init; }
}

public record LoginRequest
{
    [Required]
    public string UsernameOrEmail { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}

public record UserDto
{
    public long Id { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? FullName { get; init; }
    public string Role { get; init; } = null!;
}

public record AuthResponse
{
    public string Token { get; init; } = null!;
    public long ExpiresAt { get; init; }
    public UserDto User { get; init; } = null!;
}

public record ErrorResponse
{
    public string Message { get; init; } = null!;
}