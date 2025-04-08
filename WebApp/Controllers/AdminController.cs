using Business.Interfaces;
using Business.Models;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController(IMemberService memberService, IClientService clientService, IStatusService statusService, IProjectService projectService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;
    private readonly IProjectService _projectService = projectService;

    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllMembers();

        return View(members);
    }

    [Route("clients")]
    public async Task<IActionResult> Clients()
    {
        var vm = new ClientsViewModel
        {
            Clients = await _clientService.GetClientsAsync()

        };
        return View(vm);
    }

    [Route("projects")]
    public async Task<IActionResult> Projects()
    {
        var projects = await _projectService.GetProjectsAsync();
        var clients = await _clientService.GetClientsAsync();
        var statuses = await _statusService.GetStatusesAsync();
        var members = await _memberService.GetAllMembers();

        var addProjectForm = new AddProjectForm
        {
            // Saving lists with clients, members and statuses
            Clients = clients,
            Statuses = statuses,
            MembersOnJob = members
        };

        // creating the ViewModel
        var vm = new ProjectsViewModel
        {
            Projects = new List<Project>(),
            AddProjectForm = addProjectForm,
            EditProjectForm = new EditProjectForm()
        };

        var projectsResponse = await _projectService.GetProjectsAsync();

        if (!projectsResponse.Success)
        {
            vm.Projects = [];
        }
        else
        {
            vm.Projects = projectsResponse.Data!;
        }

        return View(vm);
    }
}
