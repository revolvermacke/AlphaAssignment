using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository repository, IProjectMemberService projectMemberService, IClientService clientService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = repository;
    private readonly IProjectMemberService _projectMemberService = projectMemberService;
    private readonly IClientService _clientService = clientService;




    public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            return ResponseResult.BadRequest("Invalid form");

        try
        {
            var projectExist = await _projectRepository.AlreadyExistsAsync(x => x.ProjectName == form.ProjectName);
            if (projectExist)
                return ResponseResult.Error("Project with that name already exist");

            await _projectRepository.BeginTransactionAsync();

            var projectEntity = ProjectFactory.Create(form);

            var result = await _projectRepository.AddAsync(projectEntity);
            var saveResult = await _projectRepository.SaveAsync();
            if (result == null || saveResult == false)
                throw new Exception("Error saving project");

            await _projectRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error creating project :: {ex.Message}");
        }
    }

    public async Task<IResponseResult> GetAllProjects()
    {
        try
        {
            var entities = await _projectRepository.GetAllAsync();
            var projects = entities.Select(e => ProjectFactory.CreateModel(e));
            return ResponseResult<IEnumerable<Project>>.Ok(projects);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error("Error retrieving projects");
        }
    }

    public async Task<IResponseResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var projectEntities = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                includes => includes.ProjectMembers,
                includes => includes.Status,
                includes => includes.Client
            );

        if (projectEntities != null && projectEntities.Any())
        { 
            return ResponseResult<IEnumerable<Project>>.Ok(projectEntities.Select(ProjectFactory.CreateModel));

        }

        return ResponseResult<IEnumerable<Project>>.Error(null, "Something went wrong");
        

    }

    public async Task<IResponseResult> GetProjectByIdAsync(string id)
    {
        try
        {
            var entity = await _projectRepository.GetAsync(x => x.Id == id);
            if (entity == null)
                return ResponseResult.NotFound("Project");

            var model = ProjectFactory.CreateModel(entity);
            return ResponseResult<Project>.Ok(model);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error("Error retreiving project.");
        }
    }

    public async Task<IResponseResult<Project>> GetProjectAsync(string id)
    {
        try
        {
            var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == id,
                includes => includes.ProjectMembers,
                includes => includes.Status,
                includes => includes.Client
            );

            if (response == null)
                throw new Exception("Error retrieving project.");

            return ResponseResult<Project>.Ok(response);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }

    //update
    public async Task<IResponseResult> UpdateProjectAsync(ProjectRegistrationForm updateForm, string id)
    {
        if (updateForm == null)
            return ResponseResult.BadRequest("Invalid form");

        try
        {
            var projectToUpdate = await _projectRepository.GetAsync(x => x.Id == id);
            if (projectToUpdate == null)
                return ResponseResult.NotFound("Project not found");

            await _projectRepository.BeginTransactionAsync();
            ProjectFactory.UpdateEntity(projectToUpdate, updateForm);
            await _projectRepository.UpdateAsync(x => x.Id == id, projectToUpdate);
            var saveResult = await _projectRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving updated project");

            var existingMemberIds = projectToUpdate.ProjectMembers
                .Select(junctionTable => junctionTable.UserId)
                .ToList();
            if (updateForm.Members != null && existingMemberIds != null)
            {
                var projectMembersUpdate = await _projectMemberService.UpdateProjectMemberAsync(id, existingMemberIds, updateForm.Members);
                if (projectMembersUpdate.Success == false)
                    throw new Exception("Error updating ProjectServices");
            }   

            await _projectRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error updating project :: {ex.Message}");
        }
    }

    //delete
    public async Task<IResponseResult> DeleteProjectAsync(string id)
    {
        try
        {
            await _projectRepository.BeginTransactionAsync();

            await _projectRepository.DeleteAsync(x => x.Id == id);

            var saveResult = await _projectRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving changes.");

            await _projectRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error deleting project :: {ex.Message}");
        }
    }
}
