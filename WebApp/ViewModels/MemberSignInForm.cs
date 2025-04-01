using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class MemberSignInForm
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]

    public string Password { get; set; } = null!;

    public static implicit operator MemberSignInDto(MemberSignInForm model)
    {
        return model == null
            ? null!
            : new MemberSignInDto
            {
                Email = model.Email.ToLower(),
                Password = model.Password,
            };
    }
}
