using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ProjectMemberRepository(DataContext context) : BaseRepository<ProjectMemberJunctionEntity, ProjectMember>(context), IProjectMemberRepository
{
}