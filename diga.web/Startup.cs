using diga.bl.Models;
using diga.dal;
using diga.dal.Seeders;
using diga.dal.Services;
using diga.web.Auth;
using diga.web.Helpers;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using AutoMapper;
using diga.web.HostedServices;
using diga.web.Options;
using diga.web.Hubs;
using diga.web.Models.Hubs;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using DinkToPdf;
using diga.web.Jobs;

namespace diga.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
      .AddControllersWithViews()
      .AddRazorRuntimeCompilation();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["authorization"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

            services.AddControllersWithViews().AddSessionStateTempDataProvider().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<DigaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DigaDatabase")));

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<DigaContext>();

            services.AddSingleton<ICultureService, CultureService>();
            services.AddSingleton<ILanguageStorage, LanguageStorage>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/Account/Login");
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder/*.AllowAnyOrigin()*/
                    .WithOrigins("https://pec.pt", "http://pec.pt", "https://diga.pt", "http://promo.diga.pt", "http://new.diga.pt", "http://localhost:53494", "https://saas.diga.pt")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diga.API", Version = "v1" });
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddSignalR();

            services.Configure<EmailOptions>(Configuration.GetSection("EmailOptions"));
            services.Configure<DomainOptions>(Configuration.GetSection("DomainOptions"));
            services.Configure<SaasOptions>(Configuration.GetSection("SaasOptions"));
            services.Configure<MangoPayOptions>(Configuration.GetSection("MangoPayOptions"));
            services.Configure<ConstantOptions>(Configuration.GetSection("Constants"));

            services.AddServices();
            services.AddDataServices();
            services.AddValidators();
            services.AddHelpers();
            services.AddSeeders();
            services.AddJobs();

            services.AddSingleton<HubUserCollection>();

            services.AddAutoMapper(typeof(Startup));
            services.AddHostedService<CheckArchiveService>();
            services.AddHostedService<DeferredService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DigaContext context, IBackgroundJobClient backgroundJobs, DigaContextSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            context.Database.Migrate();

            if (!context.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                context.Database.Migrate();
                //using (var scope = host.Services.CreateScope())
                //{
                //    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                //    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                //    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                //    // Create the Db if it doesn't exist and applies any pending migration.
                //    dbContext.Database.Migrate();

                //    // Seed the Db
                //    DbSeeder.Seed(dbContext, roleManager, userManager);
                //}
            }

            if (env.IsDevelopment() || env.IsStaging())
                seeder.Seed();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCors("CorsPolicy");
            app.UseAuthentication();    // аутентификация

            app.UseSession();

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Assets")),
                RequestPath = new PathString("/Assets")
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", " V1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ApplicationHub>("/hub");
            });

            app.UseHangfireDashboard("/hangfirejobs", new DashboardOptions()
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });

            //adding hangfire jobs
            RecurringJob.AddOrUpdate<IEupagoHelper>(x => x.CheckPayments(), Cron.Hourly);
            RecurringJob.AddOrUpdate<IHangFireMangoPayOutsJob>(x => x.Check(), Cron.Hourly);
            RecurringJob.AddOrUpdate<IHangFireMangoPayTransfersJob>(x => x.Check(), Cron.Hourly);
        }
    }
}
