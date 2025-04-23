using Domain.Models;

namespace Domain.Dtos;

public class ProjectRegistrationForm
{
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }
    public int StatusId { get; set; }
    public string ClientId { get; set; } = null!;
    public List<string> Members { get; set; } = [];
}
