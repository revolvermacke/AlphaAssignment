namespace Domain.Dtos;

public class ClientRegistrationForm
{
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Location { get; set; }
    public DateTime? CreatedDate { get; set; }
}
