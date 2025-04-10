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

public class ProjectService(IProjectRepository repository, IProjectMemberRepository projectMemberRepository, IProjectMemberService projectMemberService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = repository;
    private readonly IProjectMemberRepository _projectMemberRepository = projectMemberRepository;
    private readonly IProjectMemberService _projectMemberService = projectMemberService;

    // Using method below this one.

    //public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm form)
    //{
    //    if (form == null)
    //        return ResponseResult.BadRequest("Invalid form");

    //    try
    //    {
    //        var projectExist = await _projectRepository.AlreadyExistsAsync(x => x.ProjectName == form.ProjectName);
    //        if (projectExist)
    //            return ResponseResult.Error("Project with same name already exists.");

    //        await _projectRepository.BeginTransactionAsync();
    //        var projectEntity = form.MapTo<ProjectEntity>();
    //        var result = await _projectRepository.AddAsync(projectEntity);
    //        var saveResult = await _projectRepository.SaveAsync();
    //        if (result == null && saveResult == false)
    //            throw new Exception("Error saving project");

    //        foreach (var memberIds in form.ProjectMember.UserId)
    //        {
    //            var projectMemberEntity = ProjectMemberFactory.CreateEntity(projectEntity.Id, memberIds.ToString());
    //            await _projectMemberRepository.AddAsync(projectMemberEntity);
    //        }

    //        return ResponseResult<ProjectEntity>.Ok(result);
    //    }

    //    catch (Exception ex)
    //    {
    //        await _projectRepository.RollbackTransactionAsync();
    //        Debug.WriteLine(ex.Message);
    //        return ResponseResult.Error(ex.Message);
    //    }
    //}

    public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            return ResponseResult.BadRequest("Invalid form");

        try
        {
            var projectExist = await _projectRepository.AlreadyExistsAsync(x => x.ProjectName == form.ProjectName);
            if (projectExist == true)
                return ResponseResult.Error("Project with that name already exist");

            await _projectRepository.BeginTransactionAsync();
            var projectEntity = form.MapTo<ProjectEntity>();
            var result = await _projectRepository.AddAsync(projectEntity);
            var saveResult = await _projectRepository.SaveAsync();
            if (result == null && saveResult == false)
                throw new Exception("Error saving project");

            var currentProject = await _projectRepository.GetAsync(x => x.ProjectName == form.ProjectName);
            foreach (string memberIds in form.MemberIds)
            {
                await _projectMemberService.CreateProjectMemberAsync(currentProject.Id, memberIds);

            }

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


    public async Task<IResponseResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                includes => includes.ProjectMembers,
                includes => includes.Status,
                includes => includes.Client
            );

        return ResponseResult<IEnumerable<Project>>.Ok(response);
    }

    public async Task<IResponseResult> GetProjectByIdAsync(string id)
    {
        try
        {
            var entity = await _projectRepository.GetAsync(x => x.Id == id);
            if (entity == null)
                return ResponseResult.NotFound("Project");

            var result = entity.MapTo<Project>();
            return ResponseResult<Project>.Ok(result);
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

    //delete
    public async Task<IResponseResult> DeleteProjectAsync(string id)
    {
        try
        {
            var entity = _projectRepository.GetAsync(x => x.Id == id);
            if (entity == null)
                return ResponseResult.NotFound("project wasnt found");

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
