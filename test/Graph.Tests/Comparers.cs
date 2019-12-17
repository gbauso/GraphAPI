using Graph.Infrastructure.Database.Command.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Graph.Tests.Comparers
{
    public class UserComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            return x.Email.ToLower() == y.Email.ToLower();
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.Email.GetHashCode();
        }
    }

    public class ProjectComparer : IEqualityComparer<Project>
    {
        public bool Equals([AllowNull] Project x, [AllowNull] Project y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Project obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class TaskComparer : IEqualityComparer<Task>
    {
        public bool Equals([AllowNull] Task x, [AllowNull] Task y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Task obj)
        {
            return obj.Id.GetHashCode();
        }
    }


}
