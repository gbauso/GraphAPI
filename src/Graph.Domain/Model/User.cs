using Graph.CrossCutting.Exceptions;
using Graph.CrossCutting.Interfaces;
using Graph.Domain.Model;
using Graph.Domain.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Domain
{
    public class User : IDomain
    {
        public User(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Projects = new List<Project>();
        }
        public User(Guid id, string name, string email, ICollection<Project> projects)
        {
            Id = id;
            Name = name;
            Email = email;
            Projects = projects;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public ICollection<Project> Projects { get; private set; }
        public DomainState State { get; private set; }
        public void SetPersonalInfo(string name, string email)
        {
            this.Name = name;
            this.Email = email;

            this.Validate();
        }

        public void SetStateForRelation(bool isInsert)
        {
            State = (isInsert) ? DomainState.ADD_RELATION : DomainState.REMOVE_RELATION;
        }

        public void AddProject(Project project)
        {
            Projects.Add(project);
        }

        public void RemoveProject(Project project)
        {
            Projects.Remove(Projects.FirstOrDefault(i => i.Id == project.Id));
        }

        public void Validate()
        {
            var validator = new UserValidator();

            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid) throw new ValidationException(string.Join(";", validationResult.Errors.Select(i => i.ErrorCode)));
        }
    }
}
