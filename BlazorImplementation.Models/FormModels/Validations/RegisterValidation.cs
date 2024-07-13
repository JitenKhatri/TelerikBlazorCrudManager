using FluentValidation;

namespace BlazorImplementation.Models.FormModels.Validations
{
    public class RegisterValidation : AbstractValidator<RegisterFormModel>
    {
        public RegisterValidation() {
            RuleFor(x => x.UserName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Username is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Password is required.")
                .Matches("^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$")
                .WithMessage("Password must be at least 8 characters long, must contain an UpperCase Letter, LowerCase Letter, a special character and a Number.");

            RuleFor(x => x.ConfirmPassword).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Password and Re-Entered Password must match.");

            RuleFor(x => x.FirstName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("First Name is required.")
                .Matches(@"^(?!\s*$).{3,20}$").WithMessage("The First Name must be between 3 and 20 characters long and cannot be empty or whitespace.");

            RuleFor(x => x.LastName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Last Name is required.")
                .Matches(@"^(?!\s*$).{3,20}$").WithMessage("The Last Name must be between 3 and 20 characters long and cannot be empty or whitespace.");

            RuleFor(x => x.DateOfBirth).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .Must(BeAValidAge).WithMessage("You should be at least 20 years old and at max 80 years old.");

            RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").WithMessage("Please enter a valid Phone Number.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.");

            RuleFor(x => x.CountryId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Country is required");

            RuleFor(x => x.CityId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("City is required");
        }

        private bool BeAValidAge(DateTime? date)
        {
            if (!date.HasValue) return false;
            var age = DateTime.Today.Year - date.Value.Year;
            return age >= 20 && age <= 80;
        }
    }
}
