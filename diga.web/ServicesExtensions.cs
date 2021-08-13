using diga.dal.DbServices.PlatformPurchaseDbServises;
using diga.dal.DbServices.ProductDbServices;
using diga.web.Helpers;
using diga.web.Services.AccountServices;
using diga.web.Services.AdminServices.AdminPlatformCategoryServices;
using diga.web.Services.AdminServices.AdminPlatformContractServices;
using diga.web.Services.AdminServices.AdminPlatformUserRatingServices;
using diga.web.Services.AdminServices.AdminUserServices;
using diga.web.Services.DocumentServices.EstimateService;
using diga.web.Services.MangoPayServices;
using diga.web.Services.PlatformBalanceServices;
using diga.web.Services.PlatformCategoryServices;
using diga.web.Services.PlatformChatServices;
using diga.web.Services.PlatformCityServices;
using diga.web.Services.PlatformCompanyServices;
using diga.web.Services.PlatformContractBidServices;
using diga.web.Services.PlatformContractChangeServices;
using diga.web.Services.PlatformContractCloneServices;
using diga.web.Services.PlatformContractFavoriteServices;
using diga.web.Services.PlatformContractPriorityServices;
using diga.web.Services.PlatformContractReviewServices;
using diga.web.Services.PlatformContractServices;
using diga.web.Services.PlatformContractSigningServices;
using diga.web.Services.PlatformContractTypeServices;
using diga.web.Services.PlatformContractViewServices;
using diga.web.Services.PlatformCountryServices;
using diga.web.Services.PlatformEstimateServices;
using diga.web.Services.PlatformLanguageServices;
using diga.web.Services.PlatformNotificationServices;
using diga.web.Services.PlatformPortfolioAlbumServices;
using diga.web.Services.PlatformPortfolioVideoServices;
using diga.web.Services.PlatformPurchaseServices;
using diga.web.Services.PlatformPublicProfile;
using diga.web.Services.PlatformTagServices;
using diga.web.Services.PlatformUserRatingServices;
using diga.web.Services.PlatformUserSettingsServices;
using diga.web.Services.PlatformVATServices;
using diga.web.Services.ProductServices;
using diga.web.Services.RolesServices;
using diga.web.Services.SupportServices.SupportPlatformContractReviewServices;
using diga.web.Services.UserServices;
using diga.web.Validators.Account;
using diga.web.Validators.PlatformContracts;
using Microsoft.Extensions.DependencyInjection;
using diga.web.Services.PlatformMeasurementReportServices;
using diga.web.Services.PlatformVerificationServices;
using diga.web.Services.AdminServices.AdminPlatformVerificationServices;
using diga.web.Services.PlatformContractDiscussionServices;
using diga.web.Services.PlatformProcessingServices;
using diga.web.Jobs;
using diga.web.Services.PlatformContractPaymentPartServices;

namespace diga.web
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAdminPlatformCategoryService, AdminPlatformCategoryService>();
            services.AddTransient<IAdminPlatformContractService, AdminPlatformContractService>();
            services.AddTransient<IAdminPlatformUserRatingService, AdminPlatformUserRatingService>();
            services.AddTransient<IAdminPlatformVerificationService, AdminPlatformVerificationService>();
            services.AddTransient<IAdminUserService, AdminUserService>();

            services.AddTransient<ISupportPlatformContractReviewService, SupportPlatformContractReviewService>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPlatformBalanceService, PlatformBalanceService>();
            services.AddTransient<IPlatformCategoryService, PlatformCategoryService>();
            services.AddTransient<IPlatformChatService, PlatformChatService>();
            services.AddTransient<IPlatformContractChangeService, PlatformContractChangeService>();
            services.AddTransient<IPlatformCityService, PlatformCityService>();
            services.AddTransient<IPlatformCompanyService, PlatformCompanyService>();
            services.AddTransient<IPlatformContractBidService, PlatformContractBidService>();
            services.AddTransient<IPlatformContractCloneService, PlatformContractCloneService>();
            services.AddTransient<IPlatformContractDiscussionService, PlatformContractDiscussionService>();
            services.AddTransient<IPlatformContractPriorityService, PlatformContractPriorityService>();
            services.AddTransient<IPlatformContractReviewService, PlatformContractReviewService>();
            services.AddTransient<IPlatformContractService, PlatformContractService>();
            services.AddTransient<IPlatformContractFavoriteService, PlatformContractFavoriteService>();
            services.AddTransient<IPlatformCountryService, PlatformCountryService>();
            services.AddTransient<IPlatformEstimateService, PlatformEstimateService>();
            services.AddTransient<IPlatformLanguageService, PlatformLanguageService>();
            services.AddTransient<IPlatformNotificationService, PlatformNotificationService>();
            services.AddTransient<IPlatformPortfolioAlbumService, PlatformPortfolioAlbumService>();
            services.AddTransient<IPlatformPortfolioVideoService, PlatformPortfolioVideoService>();
            services.AddTransient<IPlatformTagService, PlatformTagService>();
            services.AddTransient<IPlatformContractTypeService, PlatformContractTypeService>();
            services.AddTransient<IPlatformUserRatingService, PlatformUserRatingService>();
            services.AddTransient<IPlatformContractViewService, PlatformContractViewService>();
            services.AddTransient<IPlatformUserSettingsService, PlatformUserSettingsService>();
            services.AddTransient<IPlatformVATService, PlatformVATService>();
            services.AddTransient<IPlatformVerificationService, PlatformVerificationService>();
            services.AddTransient<IPlatformContractSigningService, PlatformContractSigningService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IPaidFeatureDbService, PaidFeatureDbService>();
            services.AddTransient<IPaidFeatureService, PaidFeatureService>();
            services.AddTransient<IPlatformPurchaseDbService, PlatformPurchaseDbService>();
            services.AddTransient<IPlatformPurchaseService, PlatformPurchaseService>();

            services.AddTransient<IDocumentEstimateService, DocumentEstimateService>();
            services.AddTransient<IPlatformPublicProfileService, PlatformPublicProfileService>();
            services.AddTransient<IMangoPayUserService, MangoPayUserService>();

            services.AddTransient<IPlatformMeasurementReportService, PlatformMeasurementReportService>();
            services.AddTransient<IMangoPayPayInService, MangoPayPayInService>();
            services.AddTransient<IPlatformProcessingService, PlatformProcessingService>();
            services.AddTransient<IMangoPayPayOutService, MangoPayPayOutService>();
            services.AddTransient<IMangoPayTransferService, MangoPayTransferService>();

            services.AddTransient<IPlatformContractPaymentPartService, PlatformContractPaymentPartService>();

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<EmailHelper>();
            services.AddTransient<PasswordHelper>();
            services.AddTransient<PlatformContractHelper>();
            services.AddTransient<SaasHelper>();
            services.AddTransient<IPaymentHelper, PaymentHelper>();
            services.AddTransient<IEupagoHelper, EupagoHelper>();
            services.AddTransient<MangoPayHelper>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddTransient<SignUpRequestValidator>();
            services.AddTransient<PlatformContractCompleteValidator>();
            services.AddTransient<PlatformContractDatesValidator>();
            services.AddTransient<PlatformContractInfoValidator>();

            return services;
        }

        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddTransient<IHangFireMangoPayOutsJob, HangFireMangoPayOutsJob>();
            services.AddTransient<IHangFireMangoPayTransfersJob, HangFireMangoPayTransfersJob>();

            return services;
        }
    }
}
