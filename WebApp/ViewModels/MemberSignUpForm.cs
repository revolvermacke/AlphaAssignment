using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class MemberSignUpForm
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "You must enter a valid email")]
    public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$", ErrorMessage = "You must enter a stronger password")]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms & Conditions", Prompt = "I accept the terms & conditions.")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
    public bool TermsAndConditions { get; set; }

    public static implicit operator MemberRegistrationForm(MemberSignUpForm model)
    {
        return model == null
            ? null!
            : new MemberRegistrationForm
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Password = model.Password,
                RoleName = "User"
            };
    }
}
