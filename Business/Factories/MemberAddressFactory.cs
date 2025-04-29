using Data.Entities;
using Domain.Dtos;

namespace Business.Factories;

public class MemberAddressFactory
{
    public static MemberAddressEntity CreateEntity(MemberRegistrationForm form, string userId) => new()
    {
        UserId = userId,
        StreetName = form.StreetName!,
        City = form.City!,
        PostalCode = form.PostalCode!,
    };

    public static void UpdateMemberAddressEntity(MemberAddressEntity currentEntity, MemberRegistrationForm updateForm, string memberId)
    {
        currentEntity.StreetName = updateForm.StreetName!;
        currentEntity.PostalCode = updateForm.PostalCode!;
        currentEntity.City = updateForm.City!;
        currentEntity.UserId = memberId;
    }
}