namespace Business.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IResponseResult> CreateProjectMemberAsync(string projectId, string memberId);
        Task<IResponseResult> DeleteProjectMemberAsync(string projectId, string memberId);
        Task<IResponseResult> UpdateProjectMemberAsync(string projectId, List<string> currentMemberIds, List<string> newMemberIds);
    }
}