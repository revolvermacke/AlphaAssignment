using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController(IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;

    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllMembers();

        return View(members);
    }

    [Route("clients")]
    public IActionResult Clients()
    {
        return View();
    }

    [Route("projects")]
    public IActionResult Projects()
    {
        return View();
    }
}
