using Business.Interfaces;
using Business.Models;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class MembersController(IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;

    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberForm form)
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

        MemberRegistrationForm dto = form;

        var result = await _memberService.CreateMemberAsync(dto);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { success = false, message = result.ErrorMessage });

        return RedirectToAction("Members", "Admin");
    }

    [HttpPost]
    public IActionResult EditMember(EditMemberForm form)
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

    [HttpGet]
    public async Task<IActionResult> DeleteMember(string id)
    {
        try
        {
            var result = await _memberService.RemoveMemberAsync(id);

            return result.StatusCode switch
            {
                200 => RedirectToAction("Members", "Admin"),
                _ => RedirectToAction("Members", "Admin")
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, new { message = "Error deleting member" });
        }
    }

}
