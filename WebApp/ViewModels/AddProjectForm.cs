using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class AddProjectForm
{
    [Display(Name = "Client Image", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Enter a description")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Description { get; set; } = null!;

    [Display(Name = "Start Date", Prompt = "Enter a start date")]
    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "Enter a end date")]
    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Members", Prompt = "Choose members")]
    [DataType(DataType.Text)]
    public string Members { get; set; } = null!;

    [Display(Name = "Status", Prompt = "Choose status")]
    [DataType(DataType.Text)]
    public int Status { get; set; }

    [Display(Name = "Price", Prompt = "Enter a price")]
    [DataType(DataType.Text)]
    public decimal price { get; set; }

    public static implicit operator ProjectRegistrationForm(AddProjectForm model)
    {
        return model == null
            ? null!
            : new ProjectRegistrationForm
            {
                ProjectName = model.ProjectName,
                ClientId = model.ClientName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.price,
                StatusId = model.Status,
            };
    }



    public IEnumerable<Client> Clients { get; set; } = [];
    public IEnumerable<Status> Statuses { get; set; } = [];
    public IEnumerable<Member> MembersOnJob { get; set; } = [];

}
