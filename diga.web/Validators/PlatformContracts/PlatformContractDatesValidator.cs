using diga.web.Models.PlatformContracts;
using FluentValidation;

namespace diga.web.Validators.PlatformContracts
{
    public class PlatformContractDatesValidator : AbstractValidator<PlatformContractDatesDto>
    {
        public PlatformContractDatesValidator()
        {
            RuleFor(request => request.PublishDate)
                .NotNull().WithMessage("Publish date cannot be empty!");

            RuleFor(request => request.TenderEndDate)
                .NotNull().WithMessage("Tender end date cannot be empty!");
        }
    }
}
