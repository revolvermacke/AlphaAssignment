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
}
