using diga.bl.Models;
using diga.bl.Models.Platform.Chats;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.bl.Models.Platform.Information;
using diga.bl.Models.Platform.MangoPay;
using diga.bl.Models.Platform.MeasurementReport;
using diga.bl.Models.Platform.Notifications;
using diga.bl.Models.Platform.PaidFeatures;
using diga.bl.Models.Platform.PlatformPurchases;
using diga.bl.Models.Platform.Portfolio;
using diga.bl.Models.Platform.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace diga.dal
{
    public class DigaContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public DigaContext(DbContextOptions<DigaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>().HasOne(r => r.Role)
                .WithMany(r => r.UserRoles).HasForeignKey(r => r.RoleId);

            modelBuilder.Entity<ApplicationUserRole>().HasOne(r => r.User)
                .WithMany(r => r.UserRoles).HasForeignKey(r => r.UserId);

            modelBuilder.Entity<TariffModules>()
                .HasKey(c => new { c.ModuleId, c.TariffId });
            modelBuilder.Entity<UserTariffs>()
                .HasKey(c => new { c.UserId, c.TariffId });
            modelBuilder.Entity<UserModules>()
                .HasKey(c => new { c.UserId, c.ModuleId });
            modelBuilder.Entity<PacketModules>()
                .HasKey(c => new { c.PacketId, c.ModuleId });
            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(c => new { c.RoleId, c.UserId });

            modelBuilder.Entity<PlatformContract>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<PlatformContract>()
                .HasIndex(c => c.Status);
            modelBuilder.Entity<PlatformContractCategory>()
                .HasKey(c => new { c.PlatformCategoryId, c.PlatformContractId });
            modelBuilder.Entity<PlatformContractView>()
                .HasKey(c => new { c.PlatformContractId, c.ApplicationUserId });
            modelBuilder.Entity<PlatformContractTag>()
                .HasKey(c => new { c.PlatformContractId, c.PlatformTagId });
            modelBuilder.Entity<PlatformUserLanguage>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformLanguageId });
            modelBuilder.Entity<PlatformUserCategory>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformCategoryId });
            modelBuilder.Entity<PlatformUserTag>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformTagId });
            modelBuilder.Entity<PlatformAlbumCategory>()
                .HasKey(c => new { c.PortfolioAlbumId, c.PlatformCategoryId });

            modelBuilder.Entity<PlatformContractBid>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformContractId });
            modelBuilder.Entity<PlatformContractBid>().HasIndex(c => c.Status);

            modelBuilder.Entity<PlatformContractFavorite>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformContractId });
            modelBuilder.Entity<PlatformUserFilterCategory>()
                .HasKey(c => new { c.PlatformUserSettingsId, c.PlatformCategoryId });

            modelBuilder.Entity<PlatformChatRoomUser>()
                .HasKey(c => new { c.ApplicationUserId, c.PlatformChatRoomId });

            modelBuilder.Entity<PlatformEstimateVAT>()
                .HasKey(c => new { c.PlatformEstimateId, c.PlatformVATId });

            modelBuilder.Entity<PlatformUserVerification>(e =>
            {
                e.HasIndex(v => v.Created);
                e.HasIndex(v => v.Status);
            });

            modelBuilder.Entity<PlatformContract>().HasMany(t => t.PlatformEstimates).WithOne(p => p.PlatformContract).HasForeignKey(p => p.PlatformContractId);
            modelBuilder.Entity<PlatformContract>().HasOne(t => t.MainEstimate).WithMany().HasForeignKey(t => t.MainEstimateId);
        }

        public DbSet<Advantages> Advantages { get; set; }
        public DbSet<Cases> Cases { get; set; }
        public DbSet<Functions> Functions { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Sectors> Sectors { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }
        public DbSet<Texts> Texts { get; set; }

        public DbSet<Articles> Articles { get; set; }
        public DbSet<ModuleSections> ModuleSections { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Tariffs> Tariffs { get; set; }
        public DbSet<TariffModules> TariffModules { get; set; }
        public DbSet<UserTariffs> UserTariffs { get; set; }
        public DbSet<UserModules> UserModules { get; set; }
        public DbSet<Promocodes> Promocodes { get; set; }
        public DbSet<Packets> Packets { get; set; }
        public DbSet<PacketModules> PacketModules { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public DbSet<PaidFeature> PaidFeatures { get; set; }
        public DbSet<PlatformPurchase> PlatformPurchases { get; set; }

        #region Platform

        public DbSet<PlatformBalance> PlatformBalances { get; set; }
        public DbSet<PlatformCity> PlatformCities { get; set; }
        public DbSet<PlatformContract> PlatformContracts { get; set; }
        public DbSet<PlatformContractApproval> PlatformContractApprovals { get; set; }
        public DbSet<PlatformCategory> PlatformCategories { get; set; }
        public DbSet<PlatformContractCategory> PlatformContractCategories { get; set; }
        public DbSet<PlatformContractPriority> PlatformContractPriorities { get; set; }
        public DbSet<PlatformContractType> PlatformContractTypes { get; set; }
        public DbSet<PlatformCountry> PlatformCountries { get; set; }
        public DbSet<PlatformEstimate> PlatformEstimates { get; set; }
        public DbSet<PlatformEstimatePosition> PlatformEstimatePositions { get; set; }
        public DbSet<PlatformEstimateSection> PlatformEstimateSections { get; set; }
        public DbSet<PlatformContractFile> PlatformContractFiles { get; set; }
        public DbSet<PlatformContractView> PlatformContractViews { get; set; }
        public DbSet<PlatformTag> PlatformTags { get; set; }
        public DbSet<PlatformUserCategory> PlatformUserCategories { get; set; }
        public DbSet<PlatformUserFilterCategory> PlatformUserFilterCategories { get; set; }
        public DbSet<PlatformContractTag> PlatformContractTags { get; set; }
        public DbSet<PlatformUserRating> PlatformUserRatings { get; set; }
        public DbSet<PlatformUserSettings> PlatformUserSettings { get; set; }
        public DbSet<PlatformUserTag> PlatformUserTags { get; set; }
        public DbSet<PlatformUserPhoneNumber> PlatformUserPhoneNumbers { get; set; }
        public DbSet<PlatformLanguage> PlatformLanguages { get; set; }
        public DbSet<PlatformUserLanguage> PlatformUserLanguages { get; set; }

        public DbSet<PlatformPortfolioAlbum> PlatformPortfolioAlbums { get; set; }
        public DbSet<PlatformAlbumCategory> PlatformAlbumCategories { get; set; }
        public DbSet<PlatformPortfolioPhoto> PlatformPortfolioPhotos { get; set; }
        public DbSet<PlatformPortfolioVideo> PlatformPortfolioVideos { get; set; }

        public DbSet<PlatformContractReview> PlatformContractReviews { get; set; }
        public DbSet<PlatformContractReviewMark> PlatformContractReviewMarks { get; set; }
        public DbSet<PlatformContractReviewDocument> PlatformContractReviewDocuments { get; set; }

        public DbSet<PlatformContractBid> PlatformContractBids { get; set; }
        public DbSet<PlatformContractDiscussion> PlatformContractDiscussions { get; set; }
        public DbSet<PlatformVAT> PlatformVATs { get; set; }

        public DbSet<PlatformContractFavorite> PlatformContractFavorites { get; set; }
        public DbSet<PlatformContractChange> PlatformContractChanges { get; set; }
        public DbSet<PlatformContractPaymentPart> PlatformContractPaymentParts { get; set; }

        public DbSet<PlatformNotification> PlatformNotifications { get; set; }
        public DbSet<PlatformNotificationData> PlatformNotificationDatas { get; set; }

        public DbSet<PlatformChatRoom> PlatformChatRooms { get; set; }
        public DbSet<PlatformChatRoomUser> PlatformChatRoomUsers { get; set; }
        public DbSet<PlatformChatRoomMessage> PlatformChatRoomMessages { get; set; }
        public DbSet<PlatformChatRoomMessageReaction> PlatformChatRoomMessageReactions { get; set; }
        public DbSet<PlatformEstimateVAT> PlatformEstimateVATs { get; set; }
        public DbSet<PlatformMangoPayUserBankAccount> PlatformMangoPayUserBankAccounts { get; set; }
        public DbSet<PlatformMangoPayUserKYCDocument> PlatformMangoPayUserKYCDocuments { get; set; }

        public DbSet<PlatformMeasurementReportPosition> PlatformMeasurementReportPositions { get; set; }
        public DbSet<PlatformMeasurementReport> PlatformMeasurementReports { get; set; }

        public DbSet<PlatformUserVerification> PlatformUserVerifications { get; set; }
        public DbSet<PlatformUserVerificationPhoto> PlatformUserVerificationPhotos { get; set; }
        #endregion

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DigaContext>
    {
        public DigaContext CreateDbContext(string[] args)
        {
            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{envName}.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<DigaContext>();
            var connectionString = config.GetConnectionString("DigaDatabase");
            builder.UseSqlServer(connectionString);
            return new DigaContext(builder.Options);
        }
    }
}
