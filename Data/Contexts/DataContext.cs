﻿using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }
    public virtual DbSet<MemberAddressEntity> MemberAddresses { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectMemberJunctionEntity> ProjectMembers { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StatusEntity>()
            .HasData( 
            new StatusEntity { Id = 1, StatusName = "Not started" },
            new StatusEntity { Id = 2, StatusName = "Started" },
            new StatusEntity { Id = 3, StatusName = "Completed" }
        );

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Status)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectMemberJunctionEntity>()
            .HasKey(x => new { x.ProjectId, x.UserId });

        modelBuilder.Entity<ProjectMemberJunctionEntity>()
            .HasOne(x => x.Project)
            .WithMany(x => x.ProjectMembers)
            .HasForeignKey(x => x.ProjectId);

        modelBuilder.Entity<ProjectMemberJunctionEntity>()
            .HasOne(x => x.Member)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.UserId);
    }
}