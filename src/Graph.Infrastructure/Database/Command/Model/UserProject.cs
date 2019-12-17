using Graph.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Infrastructure.Database.Command.Model
{
    public class UserProject : IModel
    {
        public Guid Id { get; set; }

        public Guid Projectid { get; set; }
        public Guid UserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }

    }
}
