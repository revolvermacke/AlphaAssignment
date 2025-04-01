using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

//Image?
public class MemberEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }

    [ProtectedPersonalData]
    public DateOnly? BirthDay { get; set; }

    [ProtectedPersonalData]
    public virtual MemberAddressEntity? Address { get; set; }

    public ICollection<ProjectMemberJunctionEntity> Projects { get; set; } = [];
}
