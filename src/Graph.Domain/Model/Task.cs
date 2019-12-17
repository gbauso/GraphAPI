using Graph.CrossCutting;
using Graph.CrossCutting.Exceptions;
using Graph.CrossCutting.Interfaces;
using Graph.Domain.Model;
using Graph.Domain.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Domain
{
    public class Task : IDomain
    {
        public Task(string description, string longDescription, DateTime deadLine, User assignee, User reporter, Project project)
        {
            Id = Guid.NewGuid();
            Description = description;
            LongDescription = longDescription;
            CreatedDate = DateTime.UtcNow;
            DeadLine = deadLine;
            Assignee = assignee;
            Reporter = reporter;
            Project = project;
            Status = TaskStatusEnum.BACKLOG;
            State = DomainState.NEW;
        }

        public Task(Guid id, string description, string longDescription, DateTime createdDate, DateTime deadLine, User assignee, User reporter, Project project, TaskStatusEnum status)
        {
            Id = id;
            Description = description;
            LongDescription = longDescription;
            CreatedDate = createdDate;
            DeadLine = deadLine;
            Assignee = assignee;
            Reporter = reporter;
            Status = status;
            Project = project;
            State = DomainState.FROM_DB;
        }

        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public string LongDescription { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime DeadLine { get; private set; }
        public User Assignee { get; private set; }
        public User Reporter { get; private set; }
        public Project Project { get; private set; }
        public TaskStatusEnum Status { get; private set; }
        public DomainState State { get; private set; }

        public void SetDescription(string description, string longDescription)
        {
            this.Description = description;
            this.LongDescription = longDescription;

            this.Validate();

            this.Project.UpdateTask(this);
        }

        public void SetStatus(TaskStatusEnum status)
        {
            this.Status = status;

            this.Validate();

            this.Project.UpdateTask(this);
        }

        public void SetDeadline(DateTime deadline)
        {
            this.DeadLine = deadline;

            this.Validate();

            this.Project.UpdateTask(this);
        }

        public void ChangeAssignee(User assignee)
        {
            assignee.Validate();

            this.Assignee = assignee;

            this.Validate();

            this.Project.UpdateTask(this);
        }

        public void Validate()
        {
            var validator = new TaskValidator();

            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid) throw new ValidationException(string.Join(";", validationResult.Errors.Select(i => i.ErrorCode)));
        }
    }
}
