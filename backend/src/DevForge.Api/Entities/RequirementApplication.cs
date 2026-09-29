namespace DevForge.Api.Entities;

public class RequirementApplication
{
    public long RequirementId { get; set; }
    public long ApplicationId { get; set; }
    public string TraceType { get; set; } = "implements";
    public DateTimeOffset CreatedAt { get; set; }

    public Requirement? Requirement { get; set; }
    public Application? Application { get; set; }
}