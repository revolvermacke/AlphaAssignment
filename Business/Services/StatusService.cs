using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;


    public async Task<IEnumerable<Status>> GetStatusesAsync()
    {
        try
        {
            var statuses = await _statusRepository.GetAllAsync();
            var result = statuses.Select(s => s.MapTo<Status>());

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<IResponseResult> GetStatusByNameAsync(string statusName)
    {
        var status = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        var result = status.MapTo<Status>();
        return ResponseResult<Status>.Ok(result);
        
    }

    public async Task<IResponseResult> GetStatusByIdAsync(int id)
    {
        var status = await _statusRepository.GetAsync(x => x.Id == id);
        var result = status.MapTo<Status>();
        return ResponseResult<Status>.Ok(result);

    }

}