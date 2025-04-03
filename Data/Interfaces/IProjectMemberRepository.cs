using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IProjectMemberRepository : IBaseRepository<ProjectMemberJunctionEntity, ProjectMember>
{
}