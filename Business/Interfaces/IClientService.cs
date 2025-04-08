using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<IResponseResult> CreateClientAsync(ClientRegistrationForm form);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<IResponseResult> GetClientByIdAsync(string id);
        Task<IResponseResult> UpdateClientAsync(string id, ClientRegistrationForm updateForm);
        Task<IResponseResult> RemoveClientAsync(string id);
    }
}