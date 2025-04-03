namespace Data.Entities;

public class ProjectMemberJunctionEntity
{
    public string ProjectId { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public MemberEntity Member { get; set; } = null!;
}