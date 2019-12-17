using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Graph.Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(i => i.Id).NotNull().NotEmpty().NotEqual(Guid.Empty).WithErrorCode("NAME-01");
            RuleFor(i => i.Email).NotNull().NotEmpty().EmailAddress().WithErrorCode("EMAIL-01");
            RuleFor(i => i.Name).NotNull().NotEmpty().WithErrorCode("NAME-01");
        }
    }
}
