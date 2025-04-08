using Domain.Models;
using Data.Entities;

namespace Data.Interfaces;

public interface IStatusRepository : IBaseRepository<StatusEntity, Status>
{
}
