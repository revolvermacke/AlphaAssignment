using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllMembers();
    Task<IResponseResult> GetMemberByIdAsync(string id);
    Task<IResponseResult> CreateMemberAsync(MemberRegistrationForm form);
    Task<IResponseResult> UpdateMemberAsync(string id, MemberRegistrationForm updateForm);
    Task<IResponseResult> RemoveMemberAsync(string id);
}