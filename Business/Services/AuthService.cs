using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class AuthService(SignInManager<MemberEntity> signInManager) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;

    public async Task<bool> SignInAsync(MemberSignInDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        return result.Succeeded;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
