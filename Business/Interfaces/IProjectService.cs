using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm form);
        Task<IResponseResult<Project>> GetProjectAsync(string id);
        Task<IResponseResult<IEnumerable<Project>>> GetProjectsAsync();
    }
}