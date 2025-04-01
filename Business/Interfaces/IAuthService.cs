using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(MemberSignInDto dto);
        Task SignOutAsync();
    }
}