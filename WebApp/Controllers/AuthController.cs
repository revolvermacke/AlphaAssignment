using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService, IMemberService memberService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly IMemberService _memberService = memberService;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;

    [Route("Signin")]
    public IActionResult SignIn()
    {

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

    #region External Authentication

    [HttpPost]
    public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("ExternalSignInCallback", "Auth", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalSignInCallBack(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/projects");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("SignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = string.Empty;
            string lastName = string.Empty;

            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }

            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var user = new MemberEntity { UserName = username, Email = email, FirstName = firstName, LastName = lastName };

            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach(var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("SignIn");
        }
    }

    #endregion
}
