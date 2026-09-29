namespace DevForge.Api.Entities;

public class Application
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? AppType { get; set; }
    public string Status { get; set; } = "active";
    public long? OwnerId { get; set; }
    public User? Owner { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}