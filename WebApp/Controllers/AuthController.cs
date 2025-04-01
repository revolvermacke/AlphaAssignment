using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService, IMemberService memberService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly IMemberService _memberService = memberService;

    [Route("Signin")]
    public IActionResult SignIn(string returnUrl = "/admin/projects")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [Route("Signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(MemberSignInForm form, string returnUrl = "/admin/projects")
    {
        ViewBag.ErrorMessage = null;

        if (ModelState.IsValid)
        {
            MemberSignInDto memberSignInDto = form;

            var result = await _authService.SignInAsync(memberSignInDto);
            if (result)
                return LocalRedirect(returnUrl);
        }

        ViewBag.ErrorMessage = "Email or password is incorrect.";
        return View(form);
    }

    public IActionResult SignUp()
    {
        ViewBag.ErrorMessage = null;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(MemberSignUpForm form)
    {
        MemberRegistrationForm dto = form;

        if (ModelState.IsValid)
        {
            var result = await _memberService.CreateMemberAsync(dto);
            if (result.Success)
                return RedirectToAction("SignIn", "Auth");
        }


        ViewBag.ErrorMessage = "Failed to create account.";
        return View(form);
    }

    public new async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("SignIn", "Auth");
    }
}
