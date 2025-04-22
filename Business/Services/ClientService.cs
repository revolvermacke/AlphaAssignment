using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    // create
    public async Task<IResponseResult> CreateClientAsync(ClientRegistrationForm form)
    {
        if (form == null)
            return ResponseResult.BadRequest("Invalid form");

        try
        {
            var clientEntity = form.MapTo<ClientEntity>();
            var result = await _clientRepository.AddAsync(clientEntity);

            if (result == null)
                return ResponseResult.BadRequest("Enter all required fields.");

            await _clientRepository.SaveAsync();
            return ResponseResult<ClientEntity>.Ok(result);
        }
        catch
        { 
            return ResponseResult.Error("Failed to create client.");
        }
    }

    // read
    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        try
        {
            var clients = await _clientRepository.GetAllAsync();
            var result = clients.Select(s => s.MapTo<Client>());

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return [];
        }
    }
    // read
    public async Task<IResponseResult> GetClientByIdAsync(string id)
    {

        var client = await _clientRepository.GetAsync(x => x.Id == id);
        var result = client.MapTo<Client>();
        return ResponseResult<Client>.Ok(result);

    }

    //update
    public async Task<IResponseResult> UpdateClientAsync(string id, ClientRegistrationForm updateForm)
    {
        if (updateForm == null)
            return ResponseResult.BadRequest("Invalid form");
        try
        {
            var entityToUpdate = await _clientRepository.GetAsync(x => x.Id == id);
            if (entityToUpdate == null)
                return ResponseResult.NotFound("Client not found");

            await _clientRepository.BeginTransactionAsync();

            ClientFactory.UpdateClientEntity(entityToUpdate, updateForm);

            await _clientRepository.UpdateAsync(x => x.Id == id, entityToUpdate);

            bool saveResult = await _clientRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving");

            await _clientRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error updating client :: {ex.Message}");
        }
    }

    //delete
    public async Task<IResponseResult> RemoveClientAsync(string id)
    {
        try
        {
            var entity = await _clientRepository.GetAsync(x => x.Id == id);
            if (entity == null)
                return ResponseResult.NotFound("Client not found");

            await _clientRepository.BeginTransactionAsync();
            await _clientRepository.DeleteAsync(x => x.Id == id);
            bool saveResult = await _clientRepository.SaveAsync();
            if (saveResult == false)
                throw new Exception("Error saving changes.");

            await _clientRepository.CommitTransactionAsync();
            return ResponseResult.Ok();
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return ResponseResult.Error($"Error deleting client :: {ex.Message}");
        }
    }

}
