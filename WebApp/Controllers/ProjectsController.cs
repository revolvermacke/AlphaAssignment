using Business.Interfaces;
using Business.Models;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers;

public class ProjectsController(IProjectService projectService, IClientService clientService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectForm form)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        ProjectRegistrationForm dto = form;

        var result = await _projectService.CreateProjectAsync(dto);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { success = false, message = result.ErrorMessage });

        return RedirectToAction("Projects", "Admin");
    }

    [HttpPost]
    public IActionResult EditProject(EditProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        //send data to clientService

        return Ok(new { success = true });
    }

    //update project method.

    [HttpGet]
    public async Task<IActionResult> DeleteProject(string id)
    {
        try
        {
            // await 
            // redirect

            var result = await _projectService.DeleteProjectAsync(id);

            return result.StatusCode switch
            {
                200 => RedirectToAction("projects", "admin"),
                _ => RedirectToAction("projects", "admin")
            };
            
          
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, new { Message = "An error occurred while deleting project" });
        }
        
    }
}
