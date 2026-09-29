namespace DevForge.Api.Entities;

public class Requirement
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = "draft";
    public string Priority { get; set; } = "medium";
    public long CreatedBy { get; set; }
    public User? Creator { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}