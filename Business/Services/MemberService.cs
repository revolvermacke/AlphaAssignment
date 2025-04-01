using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository, IMemberAddressRepository memberAddressRepository) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

    public async Task<IEnumerable<Member>> GetAllMembers()
    {
        var list = await _userManager.Users.ToListAsync();
        var members = list.Select(MemberFactory.CreateModel).ToList();

        return members;
    }

    public async Task<IResponseResult> CreateMemberAsync(MemberRegistrationForm signUpForm)
    {
        if (signUpForm is null)
            return ResponseResult.BadRequest("Invalid form");

        if (signUpForm.RoleName != "Admin" && signUpForm.RoleName != "User")
            throw new Exception("Invalid role specified.");

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var memberEntity = MemberFactory.CreateEntity(signUpForm);

            var userCreationResult = await _userManager.CreateAsync(memberEntity, signUpForm.Password ?? "BytMig123!");
            if (!userCreationResult.Succeeded)
                throw new Exception("Failed to create user");

            var addToRoleResult = await _userManager.AddToRoleAsync(memberEntity, signUpForm.RoleName);
            if (!addToRoleResult.Succeeded)
                throw new Exception("Failed to add role to user");

            if (!string.IsNullOrWhiteSpace(signUpForm.StreetName) &&
                !string.IsNullOrWhiteSpace(signUpForm.PostalCode) &&
                !string.IsNullOrWhiteSpace(signUpForm.City))
            {
                var memberAddressEntity = MemberAddressFactory.CreateEntity(signUpForm, memberEntity.Id);
                await _memberAddressRepository.AddAsync(memberAddressEntity);
                bool saveAddressResult = await _memberAddressRepository.SaveAsync();
                if (saveAddressResult == false)
                    throw new Exception("Failed to save member address.");
            }

            await _memberRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Error in CreateMemberAsync: {ex.Message}");
            return ResponseResult.Error("Error creating employee");
        }
    }
}
