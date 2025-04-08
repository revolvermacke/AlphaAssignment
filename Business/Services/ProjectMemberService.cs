using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class ProjectMemberService(IProjectMemberRepository projectMemberRepository) : IProjectMemberService
{
    private readonly IProjectMemberRepository _projectMemberRepository = projectMemberRepository;


    public async Task<IResponseResult> UpdateProjectServiceAsync(int projectId, List<string> currentMemberIds, List<string> newMemberIds)
    {
        if (currentMemberIds == null)
            return ResponseResult.BadRequest("Invalid input");

        try
        {
            if (currentMemberIds.SequenceEqual(newMemberIds))
                return ResponseResult.Ok();

            var remove = currentMemberIds.Except(newMemberIds).ToList();
            var add = newMemberIds.Except(currentMemberIds).ToList();

            foreach (string memberId in remove)
            {
                var deleteResponse = DeleteProjectServiceAsync(projectId.ToString(), memberId);
                if (deleteResponse.Result.Success == false)
                    throw new Exception("Error deleting projectservice.");
            }

            foreach (var serviceId in add)
            {
                var newProjectServiceEntity = ProjectMemberFactory.CreateEntity(projectId.ToString(), serviceId);
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
            return ResponseResult.Error($"Error deleting projectserive :: {ex.Message}");
        }
    }

    public async Task<IResponseResult> DeleteProjectServiceAsync(string projectId, string memberId)
    {
        try
        {
            var projectServiceJunctionEntity = await _projectMemberRepository.GetAsync(x => x.ProjectId == projectId && x.UserId == memberId);
            if (projectServiceJunctionEntity == null)
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
