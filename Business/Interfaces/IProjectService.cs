﻿using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm form);
        Task<IResponseResult> GetAllProjects();
        Task<IResponseResult<Project>> GetProjectAsync(string id);
        Task<IResponseResult<IEnumerable<Project>>> GetProjectsAsync();
        Task<IResponseResult> GetProjectByIdAsync(string id);
        Task<IResponseResult> DeleteProjectAsync(string id);

        Task<IResponseResult> UpdateProjectAsync(ProjectRegistrationForm updateForm, string id);
    }
}