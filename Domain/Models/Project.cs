namespace Domain.Models;

public class Project
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal? Budget { get; set; }
    public Status Status { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public ProjectMember ProjectMember { get; set; } = null!;
}
