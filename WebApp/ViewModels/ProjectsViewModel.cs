using Business.Models;
using Domain.Models;

namespace WebApp.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = [];
        public AddProjectForm AddProjectForm { get; set; } = new();
        public EditProjectForm EditProjectForm { get; set; } = new();

        //public bool HasProjects => Projects.Sucess && Projects.Data != null;
        //public bool ProjectsEmpty => Projects.Sucess && Projects.Data == null;
        //public bool ProjectsError => Projects.Data == null;
        
    }
}
