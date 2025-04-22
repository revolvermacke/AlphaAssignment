using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity CreateEntity(MemberRegistrationForm form) => new()
    {
        UserName = form.Email,
        FirstName = form.FirstName,
        LastName = form.LastName,
        Email = form.Email,
        JobTitle = form.JobTitle,
        BirthDay = form.BirthDay,
        PhoneNumber = form.PhoneNumber,
    };

    public static Member CreateModel(MemberEntity entity) => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email!,
        JobTitle = entity.JobTitle,
        BirthDay = entity.BirthDay,
        PhoneNumber = entity.PhoneNumber,
        StreetName = entity.Address?.StreetName,
        City = entity.Address?.City,
        PostalCode = entity.Address?.PostalCode,
    };

    public static void UpdateMemberEntity(MemberEntity currentEntity, MemberRegistrationForm updateForm)
    {
        currentEntity.Email = updateForm.Email;
        currentEntity.FirstName = updateForm.FirstName;
        currentEntity.LastName = updateForm.LastName;
        currentEntity.PhoneNumber = updateForm.PhoneNumber;
        currentEntity.JobTitle = updateForm.JobTitle;
    }
}
