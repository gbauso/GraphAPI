using Graph.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Infrastructure.Database.Command.Model
{
    public class Task : IModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeadLine { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AssigneeId { get; set; }
        public Guid ReporterId { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual User Assignee { get; set; }
        public virtual User Reporter { get; set; }
        public virtual Project Project { get; set; }
    }
}
