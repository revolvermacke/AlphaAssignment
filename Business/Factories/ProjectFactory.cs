﻿using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectEntity Create(ProjectRegistrationForm form)
    {
        var project = new ProjectEntity
        {
            Id = Guid.NewGuid().ToString(),
            ProjectName = form.ProjectName,
            Image = form.Image,
            Description = form.Description,
            StartDate = DateOnly.FromDateTime(form.StartDate),
            EndDate = DateOnly.FromDateTime(form.EndDate),
            StatusId = form.StatusId,
            ClientId = form.ClientId,
            Budget = form.Budget,
            Created = DateTime.Now,
        };

        project.ProjectMembers = form.MemberIds
            .Select(memberId => new ProjectMemberJunctionEntity
            {
                ProjectId = project.Id,
                UserId = memberId,
            }).ToList();

        return project;
    }


    public static Project CreateModel(ProjectEntity entity)
    {
        var project = new Project
        {
            Id = entity.Id,
            Image = entity.Image,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            
            Status = new Status
            {
                Id = entity.Status.Id,
                StatusName = entity.Status.StatusName
            },
            
            Client = new Client
            {
                Id = entity.ClientId,
                ClientName = entity.Client.ClientName
            },
            ProjectMember = new ProjectMember
            {
                UserId = entity.Id,
                MemberName = entity.ProjectMembers.Select(x => x.Member.FirstName).ToList(),
                
                
            }
        };

        return project;
    }
    
}