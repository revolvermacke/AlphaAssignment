namespace Domain.Models;

public class Member
{
    public string? Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? StreetName { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
}
