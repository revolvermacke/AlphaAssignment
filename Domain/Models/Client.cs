﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Client
{
    public string? Id { get; set; }
    public string? ClientImage { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Location { get; set; }
    public DateTime? CreatedDate { get; set; }
}
