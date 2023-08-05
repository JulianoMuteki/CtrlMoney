using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using CtrlMoney.CrossCutting.Ioc;
using CtrlMoney.Domain.Identity;
using CtrlMoney.Domain.Security;
using CtrlMoney.Infra.Context;
using CtrlMoney.UI.Web.CustomConfig;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CtrlMoney.UI.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly ConnectionStrings ConnectionStrings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // ConnectionStrings = connectionStrings.Value;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // services.AddDbContext<CtrlMoneyContext>(item => item.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<CtrlMoneyContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                                .AddEntityFrameworkStores<CtrlMoneyContext>()
                                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 7; //Pa$$w0rd

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;

                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthorization(options =>
            {
                foreach (var item in PolicyTypes.ListAllClaims)
                {
                    options.AddPolicy(item.Value.Value, policy => { policy.RequireClaim(CustomClaimTypes.DefaultPermission, item.Value.Value); });
                }
            });

            //services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            //services.AddRazorPages().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            //// services.AddAutoMapperSetup();

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddRazorPages();
            RegisterServices(services);
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddHttpClient("Tickers", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://statusinvest.com.br/");
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"); // Github API versioning
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5"); // Github requires a user-agent
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36"); // Github requires a user-agent
                httpClient.DefaultRequestHeaders.Add("Connection", "close"); // Github requires a user-agent
                httpClient.DefaultRequestHeaders.Add("DNT", "1");
            });
            string urlAPIYFinance = Configuration?.GetSection("YFinanceHost")?["Url"];
            services.AddHttpClient("YFinance", httpClient =>
            {
                //httpClient.BaseAddress = new Uri("http://172.18.194.39:5001");
                httpClient.BaseAddress = new Uri(urlAPIYFinance);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CtrlMoneyContext context)
        {
            app.UseDeveloperExceptionPage();
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     app.UseExceptionHandler(@"/Home/Error");
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo("pt-BR") }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                          name: "default",
                          pattern:
                  "{controller=Home}/{action=Index}/{id?}");
            });

            app.MigrateOfContext<CtrlMoneyContext>(context);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            InfraBootStrapperModule.RegisterServices(services);
            ApplicationBootStrapperModule.RegisterServices(services);
        }
    }
    public static class EFMigration
    {
        public static void MigrateOfContext<T>(this IApplicationBuilder app, CtrlMoneyContext context) where T : CtrlMoneyContext
        {
            var conn = app.ApplicationServices.GetService<IOptions<ConnectionStrings>>().Value;
            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            logger.LogWarning("Connectionstring: {0}", context.Database.GetConnectionString());

            if (!context.Database.CanConnect())
            {
                bool isOk = context.Database.EnsureCreated();
                if (!isOk)
                {
                    
                    logger.LogWarning("EnsureCreated failed");
                }
            }
            else if (context.Database.GetPendingMigrations().Count() > 0)
                context.Database.Migrate();
        }
    }
}
