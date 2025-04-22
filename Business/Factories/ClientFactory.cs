using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public class ClientFactory
{
    public static ClientEntity CreateEntity(ClientRegistrationForm registrationForm) => new()
    {
        ClientName = registrationForm.ClientName,
        Email = registrationForm.Email,
        PhoneNumber = registrationForm.PhoneNumber,
        Location = registrationForm.Location,
    };

    public static void UpdateClientEntity(ClientEntity currentEntity, ClientRegistrationForm updateForm)
    {
        currentEntity.Email = updateForm.Email;
        currentEntity.ClientName = updateForm.ClientName;
        currentEntity.PhoneNumber = updateForm.PhoneNumber;
        currentEntity.Location = updateForm.Location;
    }
}