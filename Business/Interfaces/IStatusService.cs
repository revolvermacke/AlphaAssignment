using Domain.Models;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task<IResponseResult> GetStatusByNameAsync(string statusName);
        Task<IResponseResult> GetStatusByIdAsync(int id);
    }
}