namespace Domain.Models;

public class ProjectMember
{
    public string ProjectId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public List<string> MemberName { get; set; } = null!;
}