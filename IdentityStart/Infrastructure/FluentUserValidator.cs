using FluentValidation;
using IdentityStart.Models;

namespace IdentityStart.Infrastructure
{
    public class FluentCreateModelValidator : AbstractValidator<CreateModel>
    {
        public FluentCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("ConfirmPassword is required");

            RuleFor(x => x)
                .Must(BeAValidPostcode)
                .WithMessage("Password and Name must not match, Email must not contain Password as well");
        }

        private bool BeAValidPostcode(object o)
        {
            var instance = o as CreateModel;
            if (instance != null && (instance.Name == instance.Password || instance.Email.Contains(instance.Password)))
            {
                return false;
            }

            return true;
        }
    }
}