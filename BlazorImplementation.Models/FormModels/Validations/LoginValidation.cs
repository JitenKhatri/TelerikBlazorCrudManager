using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorImplementation.Models.FormModels.Validations
{
    public class LoginValidation : AbstractValidator<LoginFormModel>
    {
        public LoginValidation()
        {
            RuleFor(l => l.Email).NotEmpty().WithMessage("Username is required");
            RuleFor(l => l.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(l => l.Email).EmailAddress().When(l => !string.IsNullOrEmpty(l.Email)).WithMessage("Please enter a valid Username");
        }
    }
}
