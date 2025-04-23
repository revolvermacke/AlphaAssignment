using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using System.Diagnostics;

namespace Business.Services;

public class ProjectMemberService(IProjectMemberRepository projectMemberRepository) : IProjectMemberService
{
    private readonly IProjectMemberRepository _projectMemberRepository = projectMemberRepository;


    public async Task<IResponseResult> CreateProjectMemberAsync(string projectId, string memberId)
    {
        var source = new
        {
            ProjectId = projectId,
            UserId = memberId
        };

        var projectMember = source.MapTo<ProjectMemberJunctionEntity>();

        await _projectMemberRepository.AddAsync(projectMember);
        await _projectMemberRepository.SaveAsync();

        return ResponseResult<ProjectMemberJunctionEntity>.Ok(projectMember);
    }

    public async Task<IResponseResult> UpdateProjectMemberAsync(string projectId, List<string> currentMemberIds, List<string> newMemberIds)
    {

        try
        {
            if (currentMemberIds.SequenceEqual(newMemberIds))
                return ResponseResult.Ok();

            var remove = currentMemberIds.Except(newMemberIds).ToList();
            var add = newMemberIds.Except(currentMemberIds).ToList();

            foreach (string memberId in remove)
            {
                var deleteResponse = await DeleteProjectMemberAsync(projectId, memberId);
                if (deleteResponse.Success == false)
                    throw new Exception($"Error deleting projectmember. :: {deleteResponse.ErrorMessage}");
            }

            foreach (var memberId in add)
            {
                var newProjectServiceEntity = ProjectMemberFactory.CreateEntity(projectId.ToString(), memberId);
                await _projectMemberRepository.AddAsync(newProjectServiceEntity);
                var addSaveResult = await _projectMemberRepository.SaveAsync();
                if (addSaveResult == false)
                    throw new Exception("Error saving changes");
            }

            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error updating projectmember :: {ex.Message}");
        }
    }

    public async Task<IResponseResult> DeleteProjectMemberAsync(string projectId, string memberId)
    {
        try
        {
            var projectMemberEntity = await _projectMemberRepository.GetAsync(x => x.ProjectId == projectId && x.UserId == memberId);
            if (projectMemberEntity == null)
                return ResponseResult.NotFound("Entity not found");

            await _projectMemberRepository.DeleteAsync(x => x.ProjectId == projectId && x.UserId == memberId);
            var saveResult = await _projectMemberRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error trying to save changes.");

            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error deleting entity :: {ex.Message}");
        }
    }
}
