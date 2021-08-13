using diga.web.Models.PlatformContracts;
using FluentValidation;

namespace diga.web.Validators.PlatformContracts
{
    public class PlatformContractInfoValidator : AbstractValidator<PlatformContractInfoDto>
    {
        public PlatformContractInfoValidator()
        {
            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name cannot be empty!")
                .NotEmpty().WithMessage("Name cannot be empty!");

            RuleFor(request => request.Address)
                .NotNull().WithMessage("Address cannot be empty!")
                .NotEmpty().WithMessage("Address cannot be empty!");

            RuleFor(request => request.CityId)
                .NotNull().WithMessage("City cannot be empty!")
                .NotEqual(0).WithMessage("City cannot be empty!");

            RuleFor(request => request.PriorityId)
                .NotNull().WithMessage("Priority cannot be empty!")
                .NotEqual(0).WithMessage("Priority cannot be empty!");

            RuleFor(request => request.ContractTypeId)
                .NotNull().WithMessage("Contract type cannot be empty!")
                .NotEqual(0).WithMessage("Contract type cannot be empty!");
        }
    }
}
