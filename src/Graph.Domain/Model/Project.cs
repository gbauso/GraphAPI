using Graph.CrossCutting.Exceptions;
using Graph.CrossCutting.Extensions;
using Graph.CrossCutting.Interfaces;
using Graph.Domain.Model;
using Graph.Domain.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph.Domain
{
    public class Project : IDomain
    {
        public Project(string description, string longDescription)
        {
            Id = Guid.NewGuid();
            Description = description;
            LongDescription = longDescription;
            Users = new List<User>();
            Tasks = new List<Task>();
            State = DomainState.NEW;
        }

        public Project(Guid id, string description, string longDescription, ICollection<User> users, ICollection<Task> tasks)
        {
            Id = id;
            Description = description;
            LongDescription = longDescription;
            Users = users;
            Tasks = tasks;
            State = DomainState.FROM_DB;
        }

        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public string LongDescription { get; private set; }
        public ICollection<User> Users { get; private set; }
        public ICollection<Task> Tasks { get; private set; }
        public DomainState State { get; private set; }

        public void AddUser(User user)
        {
            user.Validate();

            if (Users.Any(i => i.Id == user.Id)) throw new ValidationException("PROJUSER-01");

            user.SetStateForRelation(true);

            user.AddProject(this);
            Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            user.Validate();

            if (!Users.Any(i => i.Id == user.Id)) throw new ValidationException("PROJUSER-02");

            user.SetStateForRelation(false);

            user.RemoveProject(this);
            Users.Update(user);


        }

        public void AddTask(Task task)
        {
            task.Validate();

            if (Tasks.Any(i => i.Id == task.Id)) throw new ValidationException("PROJTASK-01");
            if (!Users.Any(i => i.Id == task.Assignee.Id) || !Users.Any(i => i.Id == task.Reporter.Id)) throw new ValidationException("PROJTASK-02");

            Tasks.Add(task);
        }

        public void UpdateTask(Task task)
        {
            task.Validate();

            var selectedTask = Tasks.FirstOrDefault(i => i.Id == task.Id);

            if (selectedTask.IsNull()) throw new ValidationException("PROJTASK-01");
            if (!Users.Any(i => i.Id == task.Assignee.Id) || !Users.Any(i => i.Id == task.Reporter.Id)) 
                throw new ValidationException("PROJTASK-02");

            Tasks.Update(task);
        }

        public void RemoveTask(Task task)
        {
            task.Validate();

            if (!Tasks.Any(i => i.Id == task.Id)) throw new ValidationException("PROJTASK-01");

            Tasks.Remove(task);
        }

        public void SetDescription(string description, string longDescription)
        {
            this.Description = description;
            this.LongDescription = longDescription;

            this.Validate();
        }

        public void Validate()
        {
            var validator = new ProjectValidator();

            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid) throw new ValidationException(string.Join(";", validationResult.Errors.Select(i => i.ErrorCode)));
        }
    }
}
