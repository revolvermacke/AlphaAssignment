using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class AddMemberForm
{
    //[Required(ErrorMessage = "Required")]
    //[Display(Name = "Role Name", Prompt = "Select a role")]
    //public string RoleName { get; set; } = null!;

    [Display(Name = "Member Image", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? MemberImage { get; set; }

    [Display(Name = "First Name", Prompt = "Enter firstname")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter lastname")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Birthday", Prompt = "Enter your birthday")]
    [Required(ErrorMessage = "You must enter your birthday")]
    public DateOnly BirthDay { get; set; }

    [Display(Name = "Job Title", Prompt = "Enter job title")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string JobTitle { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    [Display(Name = "Street Name", Prompt = "Enter street name")]
    [Required(ErrorMessage = "Required")]
    public string StreetName { get; set; } = null!;

    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [Required(ErrorMessage = "Required")]
    public string PostalCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter city")]
    [Required(ErrorMessage = "Required")]
    public string City { get; set; } = null!;

    public static implicit operator MemberRegistrationForm(AddMemberForm model)
    {
        return model == null
            ? null!
            : new MemberRegistrationForm
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                JobTitle = model.JobTitle,
                StreetName = model.StreetName,
                PostalCode = model.PostalCode,
                City = model.City,
                BirthDay = model.BirthDay,
                RoleName = "User"
            };
    }
}
