using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Kztek.Security;
using Kztek_Core.Models;
using Kztek_Data;
using Kztek_Data.Repository;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Service.Admin;
using Kztek_Web.Hubs;
using Kztek_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using Microsoft.IdentityModel.Tokens;

namespace Kztek_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Cấu hình time zone
            //var culture = CultureInfo.CreateSpecificCulture("en-US");
            //var dateformat = new DateTimeFormatInfo
            //{
            //    ShortDatePattern = "dd/MM/yyyy",
            //    LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
            //};

            //culture.DateTimeFormat = dateformat;

            //var supportedCultures = new[]
            //{
            //   culture
            //};
            //signal
           // services.AddTransient<Itbl_EventRepository, tbl_EventRepository>();

            services.AddSignalR();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
                //options.SupportedCultures = supportedCultures;
                //options.SupportedUICultures = supportedCultures;
            });

            services.AddMemoryCache();

            //Net core 2.2
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                //.AddJsonOptions(options => {
                //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //    //options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                //});

            services.AddSession();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SignalrHelper>();


            services.AddSingleton<CacheHelper>();

            //Cấu hình bảo mật API
            services.AddAuthentication()
            .AddJwtBearer(ApiConfig.Auth_Bearer_Mobile, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer_Mobile"],
                    ValidAudience = Configuration["Jwt:Issuer_Mobile"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityModel.Mobile_Key))
                };
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(ApiConfig.Auth_Bearer_Mobile)
                .Build();

                options.AddPolicy(ApiConfig.Auth_Bearer_Mobile, policy =>
                {
                    policy.AuthenticationSchemes.Add(ApiConfig.Auth_Bearer_Mobile);
                    policy.RequireAuthenticatedUser();
                });
            });

            //Chuyển db
            var connect = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;
            var connecttype = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultType").Result;

            switch (connecttype)
            {

                case DatabaseModel.SQLSERVER:

                    services.AddDbContext<Kztek_Entities>(opts => opts.UseSqlServer(connect));
                 
                    break;

                case DatabaseModel.MYSQL:

                    services.AddDbContext<Kztek_Entities>(opts => opts.UseMySQL(connect));
                  
                    break;
            }

            //Autofac

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();


            //Mapping service theo đúng cơ sở dữ liệu
            switch (connecttype)
            {
                case DatabaseModel.SQLSERVER:

                    //back-end
                    builder.RegisterAssemblyTypes(typeof(Kztek_Service.Admin.Database.SQLSERVER.UserService).Assembly)
                    .Where(t => t.Name.EndsWith("Service") && t.Namespace.Contains(DatabaseModel.SQLSERVER) && t.Namespace.Contains("Admin"))
                    .AsImplementedInterfaces().InstancePerLifetimeScope();

                    //api
                    builder.RegisterAssemblyTypes(typeof(Kztek_Service.Api.Database.SQLSERVER.UserService).Assembly)
                    .Where(t => t.Name.EndsWith("Service") && t.Namespace.Contains(DatabaseModel.SQLSERVER) && t.Namespace.Contains("Api"))
                    .AsImplementedInterfaces().InstancePerLifetimeScope();

                    break;

                case DatabaseModel.MYSQL:

                    //back-end
                    builder.RegisterAssemblyTypes(typeof(Kztek_Service.Admin.Database.MYSQL.UserService).Assembly)
                     .Where(t => t.Name.EndsWith("Service") && t.Namespace.Contains(DatabaseModel.MYSQL) && t.Namespace.Contains("Admin"))
                     .AsImplementedInterfaces().InstancePerLifetimeScope();

                    //api
                    builder.RegisterAssemblyTypes(typeof(Kztek_Service.Api.Database.MYSQL.UserService).Assembly)
                     .Where(t => t.Name.EndsWith("Service") && t.Namespace.Contains(DatabaseModel.MYSQL) && t.Namespace.Contains("Api"))
                     .AsImplementedInterfaces().InstancePerLifetimeScope();

                    break;
            }

            builder.Populate(services);

            var container = builder.Build();       

            //Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vi-VN"),
                // Formatting numbers, dates, etc.
                SupportedCultures = new List<CultureInfo> { new CultureInfo("vi-VN") },
                // UI strings that we have localized.
                SupportedUICultures = new List<CultureInfo> { new CultureInfo("vi-VN") }
            });

            app.UseDeveloperExceptionPage();
            //app.UseMvcWithDefaultRoute();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "uploads")); 

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = new PathString("/uploads")
            });

            app.UseRouting();

            app.UseSession();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SqlHub>("/sqlHub");
            });

            app.UseCookiePolicy();
        }
    }
}
