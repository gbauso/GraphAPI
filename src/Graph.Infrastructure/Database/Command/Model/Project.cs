using Graph.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Infrastructure.Database.Command.Model
{
    public class Project : IModel
    {
        public Project()
        {
            UserProjects = new List<UserProject>();
            Tasks = new List<Task>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
