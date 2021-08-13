using diga.web.Models.Account;
using FluentValidation;

namespace diga.web.Validators.Account
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequestDto>
    {
        public SignUpRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotNull().WithMessage("Email cannot be empty!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .EmailAddress().WithMessage("email is not correct!");

            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name cannot be empty!")
                .NotEmpty().WithMessage("Name cannot be empty!");

            RuleFor(request => request.Password)
                .NotNull().WithMessage("Password cannot be empty!")
                .NotEmpty().WithMessage("Password cannot be empty!");

            RuleFor(request => request.PhoneNumber)
                .NotNull().WithMessage("Phone number cannot be empty!")
                .NotEmpty().WithMessage("Phone number cannot be empty!");
        }
    }
}
