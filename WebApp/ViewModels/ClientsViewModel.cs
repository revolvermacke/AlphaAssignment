using Business.Models;
using Domain.Models;

namespace WebApp.ViewModels;

public class ClientsViewModel
{
    public IEnumerable<Client> Clients { get; set; } = [];
    public AddClientForm AddClientForm { get; set; } = new();
    public EditClientForm EditClientForm { get; set; } = new();
}
