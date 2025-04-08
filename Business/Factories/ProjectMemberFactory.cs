using Data.Entities;

namespace Business.Factories;

public class ProjectMemberFactory
{
    public static ProjectMemberJunctionEntity CreateEntity(string projectId, string userId) => new()
    {
        ProjectId = projectId,
        UserId = userId,
    };
}
