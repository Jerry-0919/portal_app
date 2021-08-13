using diga.bl.Models.Platform.Contracts;
using FluentValidation;

namespace diga.web.Validators.PlatformContracts
{
    public class PlatformContractCompleteValidator : AbstractValidator<PlatformContract>
    {
        public PlatformContractCompleteValidator()
        {
            // Info 
            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name cannot be empty!")
                .NotEmpty().WithMessage("Name cannot be empty!");

            RuleFor(request => request.Address)
                .NotNull().WithMessage("Address cannot be empty!")
                .NotEmpty().WithMessage("Address cannot be empty!");

            RuleFor(request => request.CityId)
                .NotNull().WithMessage("City cannot be empty!")
                .NotEqual(0).WithMessage("City cannot be empty!");

            RuleFor(request => request.ContractPriorityId)
                .NotNull().WithMessage("Priority cannot be empty!")
                .NotEqual(0).WithMessage("Priority cannot be empty!");

            RuleFor(request => request.ContractTypeId)
                .NotNull().WithMessage("Contract type cannot be empty!")
                .NotEqual(0).WithMessage("Contract type cannot be empty!");

            RuleFor(request => request.BalanceId)
                .NotNull().WithMessage("Balance cannot be empty!")
                .NotEqual(0).WithMessage("Balance cannot be empty!");

            // Dates
            RuleFor(request => request.PublishDate)
                .NotNull().WithMessage("Publish date cannot be empty!");

            RuleFor(request => request.TenderEndDate)
                .NotNull().WithMessage("Tender end date cannot be empty!");
        }
    }
}
