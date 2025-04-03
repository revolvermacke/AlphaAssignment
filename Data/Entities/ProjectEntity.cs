using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Image { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateOnly StartDate { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateOnly EndDate { get; set; }

    public decimal? Budget { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;


    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;


    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;
    public ClientEntity Client { get; set; } = null!;


    // users on projects
    public ICollection<ProjectMemberJunctionEntity> ProjectMembers { get; set; } = [];
}