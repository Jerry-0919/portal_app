using diga.dal.DbServices.PlatformAlbumCategoryDbServices;
using diga.dal.DbServices.PlatformBalanceDbServices;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.dal.DbServices.PlatformChatRoomDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageReactionDbServices;
using diga.dal.DbServices.PlatformChatRoomUserDbServices;
using diga.dal.DbServices.PlatformCityDbServices;
using diga.dal.DbServices.PlatformCompanyDbServices;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractDiscussionDbServices;
using diga.dal.DbServices.PlatformContractFavoriteDbServices;
using diga.dal.DbServices.PlatformContractFileDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformContractPriorityDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.dal.DbServices.PlatformContractReviewMarkDbServices;
using diga.dal.DbServices.PlatformContractTagDbServices;
using diga.dal.DbServices.PlatformContractTypeDbServices;
using diga.dal.DbServices.PlatformContractViewDbServices;
using diga.dal.DbServices.PlatformCountryDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.dal.DbServices.PlatformEstimateVATServices;
using diga.dal.DbServices.PlatformLanguageDbServices;
using diga.dal.DbServices.PlatformMangoPayUserBankAccountDbServices;
using diga.dal.DbServices.PlatformMangoPayUserKYCDocumentDbService;
using diga.dal.DbServices.PlatformMeasurementReportDbServices;
using diga.dal.DbServices.PlatformMeasurementReportPositionDbServices;
using diga.dal.DbServices.PlatformNotificationDbServices;
using diga.dal.DbServices.PlatformPortfolioAlbumDbServices;
using diga.dal.DbServices.PlatformPortfolioPhotoDbServices;
using diga.dal.DbServices.PlatformPortfolioVideoDbServices;
using diga.dal.DbServices.PlatformTagDbServices;
using diga.dal.DbServices.PlatformUserCategoryDbServices;
using diga.dal.DbServices.PlatformUserFilterCategoryDbServices;
using diga.dal.DbServices.PlatformUserLanguageDbServices;
using diga.dal.DbServices.PlatformUserPhoneNumberDbServices;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.dal.DbServices.PlatformUserSettingsDbServices;
using diga.dal.DbServices.PlatformUserTagDbServices;
using diga.dal.DbServices.PlatformUserVerificationDbServices;
using diga.dal.DbServices.PlatformVATDbServices;
using diga.dal.DbServices.RoleDbServices;
using diga.dal.DbServices.TariffDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.dal.DbServices.UserRoleServices;
using diga.dal.DbServices.UserTariffDbServices;
using diga.dal.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace diga.dal
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IPlatformAlbumCategoryDbService, PlatformAlbumCategoryDbService>();
            services.AddScoped<IPlatformBalanceDbService, PlatformBalanceDbService>();
            services.AddScoped<IPlatformCategoryDbService, PlatformCategoryDbService>();
            services.AddScoped<IPlatformChatRoomDbService, PlatformChatRoomDbService>();
            services.AddScoped<IPlatformChatRoomMessageDbService, PlatformChatRoomMessageDbService>();
            services.AddScoped<IPlatformChatRoomMessageReactionDbService, PlatformChatRoomMessageReactionDbService>();
            services.AddScoped<IPlatformChatRoomUserDbService, PlatformChatRoomUserDbService>();
            services.AddScoped<IPlatformCityDbService, PlatformCityDbService>();
            services.AddScoped<IPlatformContractApprovalDbService, PlatformContractApprovalDbService>();
            services.AddScoped<IPlatformCompanyDbService, PlatformCompanyDbService>();
            services.AddScoped<IPlatformContractBidDbService, PlatformContractBidDbService>();
            services.AddScoped<IPlatformContractDbService, PlatformContractDbService>();
            services.AddScoped<IPlatformContractDiscussionDbService, PlatformContractDiscussionDbService>();
            services.AddScoped<IPlatformContractFavoriteDbService, PlatformContractFavoriteDbService>();
            services.AddScoped<IPlatformContractFileDbService, PlatformContractFileDbService>();
            services.AddScoped<IPlatformContractPaymentPartDbService, PlatformContractPaymentPartDbService>();
            services.AddScoped<IPlatformContractTypeDbService, PlatformContractTypeDbService>();
            services.AddScoped<IPlatformContractViewDbService, PlatformContractViewDbService>();
            services.AddScoped<IPlatformContractPriorityDbService, PlatformContractPriorityDbService>();
            services.AddScoped<IPlatformContractReviewDbService, PlatformContractReviewDbService>();
            services.AddScoped<IPlatformContractReviewMarkDbService, PlatformContractReviewMarkDbService>();
            services.AddScoped<IPlatformContractTagDbService, PlatformContractTagDbService>();
            services.AddScoped<IPlatformContractCategoryDbService, PlatformContractCategoryDbService>();
            services.AddScoped<IPlatformContractChangeDbService, PlatformContractChangeDbService>();
            services.AddScoped<IPlatformCountryDbService, PlatformCountryDbService>();
            services.AddScoped<IPlatformEstimateDbService, PlatformEstimateDbService>();
            services.AddScoped<IPlatformEstimatePositionDbService, PlatformEstimatePositionDbService>();
            services.AddScoped<IPlatformEstimateSectionDbService, PlatformEstimateSectionDbService>();
            services.AddScoped<IPlatformLanguageDbService, PlatformLanguageDbService>();
            services.AddScoped<IPlatformNotificationDbService, PlatformNotificationDbService>();
            services.AddScoped<IPlatformPortfolioAlbumDbService, PlatformPortfolioAlbumDbService>();
            services.AddScoped<IPlatformPortfolioPhotoDbService, PlatformPortfolioPhotoDbService>();
            services.AddScoped<IPlatformPortfolioVideoDbService, PlatformPortfolioVideoDbService>();
            services.AddScoped<IPlatformAlbumCategoryDbService, PlatformAlbumCategoryDbService>();
            services.AddScoped<IPlatformTagDbService, PlatformTagDbService>();
            services.AddScoped<IPlatformUserCategoryDbService, PlatformUserCategoryDbService>();
            services.AddScoped<IPlatformUserFilterCategoryDbService, PlatformUserFilterCategoryDbService>();
            services.AddScoped<IPlatformUserLanguageDbService, PlatformUserLanguageDbService>();
            services.AddScoped<IPlatformUserPhoneNumberDbService, PlatformUserPhoneNumberDbService>();
            services.AddScoped<IPlatformUserRatingDbService, PlatformUserRatingDbService>();
            services.AddScoped<IPlatformUserSettingsDbService, PlatformUserSettingsDbService>();
            services.AddScoped<IPlatformUserTagDbService, PlatformUserTagDbService>();
            services.AddScoped<IPlatformVATDbService, PlatformVATDbService>();
            services.AddScoped<IRoleDbService, RoleDbService>();
            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IUserRoleDbService, UserRoleDbService>();
            services.AddScoped<IUserTariffDbService, UserTariffDbService>();
            services.AddScoped<ITariffDbService, TariffDbService>();
            services.AddScoped<IPlatformEstimateVATDbService, PlatformEstimateVATDbService>();
            services.AddScoped<IPlatformUserVerificationDbService, PlatformUserVerificationDbService>();
            services.AddScoped<IPlatformMangoPayUserBankAccountDbService, PlatformMangoPayUserBankAccountDbService>();
            services.AddScoped<IPlatformMangoPayUserKYCDocumentDbService, PlatformMangoPayUserKYCDocumentDbService>();
            services.AddScoped<IPlatformMeasurementReportDbService, PlatformMeasurementReportDbService>();
            services.AddScoped<IPlatformMeasurementReportPositionDbService, PlatformMeasurementReportPositionDbService>();

            return services;
        }

        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            services.AddScoped<DigaContextSeeder>();

            return services;
        }
    }
}
