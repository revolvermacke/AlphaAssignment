using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class EditProjectForm
{
    public string Id { get; set; } = null!;

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
    public string ClientId { get; set; } = null!;

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
    public List<string> Members { get; set; } = [];

    [Display(Name = "Price", Prompt = "Enter a price")]
    [DataType(DataType.Text)]
    public decimal Budget { get; set; }

    [Display(Name = "Status")]
    [DataType(DataType.Text)]
    public int StatusId { get; set; }
}
