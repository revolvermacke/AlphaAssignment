using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository, IMemberAddressRepository memberAddressRepository) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

    // read
    public async Task<IEnumerable<Member>> GetAllMembers()
    {
        var list = await _userManager.Users.ToListAsync();
        var members = list.Select(MemberFactory.CreateModel).ToList();

        return members;
    }

    // read
    public async Task<IResponseResult> GetMemberByIdAsync(string id)
    {
        try
        {
            var memberEntity = await _memberRepository.GetAsync(x => x.Id == id);
            if (memberEntity == null)
                return ResponseResult.NotFound("Member not found.");

            var result = memberEntity.MapTo<Member>();
            return ResponseResult<Member>.Ok(result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error(ex.Message);
        }
    }

    // create
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

    // update
    public async Task<IResponseResult> UpdateMemberAsync(string id, MemberRegistrationForm updateForm)
    {
        if (updateForm == null)
            return ResponseResult.BadRequest("Invalid form.");
        
        try
        {

            var entityToUpdate = await _memberRepository.GetAsync(x => x.Id == id);
            if (entityToUpdate == null)
                return ResponseResult.NotFound("Member not found");

            await _memberRepository.BeginTransactionAsync();
            MemberFactory.UpdateMemberEntity(entityToUpdate, updateForm);

            await _memberRepository.UpdateAsync(x => x.Id == id, entityToUpdate);

            bool saveResult = await _memberRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving changes.");

            await _memberRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error updating member :: {ex.Message}");
        }
    }

    // delete
    public async Task<IResponseResult> RemoveMemberAsync(string id)
    {
        try
        {
            var entity = await _memberRepository.GetAsync(x => x.Id == id);
            if (entity == null)
                return ResponseResult.NotFound("Member not found");

            await _memberRepository.BeginTransactionAsync();
            await _memberRepository.DeleteAsync(x => x.Id == id);
            bool saveResult = await _memberRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving changes.");

            await _memberRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error deleting member :: {ex.Message}");
        }
    }
}
