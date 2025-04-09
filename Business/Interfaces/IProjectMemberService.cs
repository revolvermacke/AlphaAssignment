namespace Business.Interfaces
{
    public interface IProjectMemberService
    {
        Task<IResponseResult> CreateProjectMemberAsync(string projectId, string memberId);
        Task<IResponseResult> DeleteProjectServiceAsync(string projectId, string memberId);
        Task<IResponseResult> UpdateProjectServiceAsync(int projectId, List<string> currentMemberIds, List<string> newMemberIds);
    }
}