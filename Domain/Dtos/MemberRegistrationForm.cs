namespace Domain.Dtos;

public class MemberRegistrationForm
{
    public string RoleName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string? JobTitle { get; set; }
    public string? StreetName { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}
