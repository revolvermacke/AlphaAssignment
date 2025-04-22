using Business.Interfaces;
using Business.Models;
using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers;

public class ClientsController(IClientService clientService) : Controller
{
    private readonly IClientService _clientService = clientService;

    [HttpPost]
    public async Task<IActionResult> AddClient(AddClientForm form)
    {

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        ClientRegistrationForm dto = form;
        var result = await _clientService.CreateClientAsync(dto);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { success = false, message = result.ErrorMessage });

        return RedirectToAction("Clients", "Admin");
    }


    [HttpPost]
    public async Task<IActionResult> EditClient(EditClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        var dto = form.MapTo<ClientRegistrationForm>();
        var updateResult = await _clientService.UpdateClientAsync(form.Id, dto);

        return updateResult.Success
            ? RedirectToAction("Clients", "Admin")
            : StatusCode(updateResult.StatusCode, new { success = false, message = updateResult.ErrorMessage });
    }

    [HttpGet]
    public async Task<IActionResult> GetClient(string id)
    {
        var result = await _clientService.GetClientByIdAsync(id);

        var client = ((ResponseResult<Client>)result).Data;

        return Json(client);
    }


    [HttpGet]
    public async Task<IActionResult> DeleteClient(string id)
    {
        try
        {
            var result = await _clientService.RemoveClientAsync(id);
            return result.StatusCode switch
            {
                200 => RedirectToAction("Clients", "Admin"),
                _ => RedirectToAction("Clients", "Admin")
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, new { Message = "An error occurred while deleting client" });
        }
    }
}
