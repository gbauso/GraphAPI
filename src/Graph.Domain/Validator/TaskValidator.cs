using FluentValidation;
using Graph.CrossCutting;
using System;
using System.Linq;

namespace Graph.Domain.Validator
{
    public class TaskValidator : AbstractValidator<Task>
    {
        public TaskValidator()
        {
            RuleFor(i => i.Id).NotNull().NotEqual(Guid.Empty).WithErrorCode("ID-01");
            RuleFor(i => i.Description).NotNull().NotEmpty().WithErrorCode("DESC-01");
            RuleFor(i => i.CreatedDate).NotNull().NotEmpty().WithErrorCode("CREATE-01");
            RuleFor(i => i.DeadLine).NotNull().NotEmpty().WithErrorCode("DEADLINE-01");
            RuleFor(i => i.DeadLine).GreaterThan((i) => i.CreatedDate).WithErrorCode("DEADLINE-02");
            RuleFor(i => i.Assignee).NotNull().WithErrorCode("ASSIGNEE-01");
            //RuleFor(i => i.Assignee).NotEqual((task) => task.Project.Users.FirstOrDefault(i => i.Id == task.Assignee.Id)).WithErrorCode("ASSIGNEE-02");
            RuleFor(i => i.Reporter).NotNull().WithErrorCode("REPORTER-01");
            RuleFor(i => i.Status).NotNull().WithErrorCode("STATUS-01");
            RuleFor(i => i.Status).IsInEnum().WithErrorCode("STATUS-02");
        }
    }
}
