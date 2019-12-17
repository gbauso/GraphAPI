using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Domain.Validator
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(i => i.Id).NotNull().NotEqual(Guid.Empty).WithErrorCode("ID-01");
            RuleFor(i => i.Description).NotNull().NotEmpty().WithErrorCode("DESC-01");
        }
    }
}
