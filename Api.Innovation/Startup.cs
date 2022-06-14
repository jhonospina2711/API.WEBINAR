using Business.Implements.Base;
using Business.Implements.General;
using Business.Interfaces.Base;
using Business.Interfaces.General;
using Data.Contexts;
using Data.Interfaces;
using Data.Providers;
using Entities.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.OpenApi.Models;
using Entities;
using API.Covid19.Validations;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Api.Covid19
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
            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Premios e innovación", Version = "v1" });
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(3),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddHttpClient();
            services.AddDbContext<SqlServerContext>();
            services.AddScoped<DbContext, SqlServerContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IResponseService, ResponseService>();
            services.AddTransient<IExceptionHandler, ExceptionHandler>();
            //services.AddTransient<IRequestServiceDetails, RequestServiceDetails>();
            services.AddTransient<IUsers, Users>();
            services.AddTransient<IUsersOneTimePass, UsersOneTimePass>();
            services.AddTransient<IUserWalletAccounts, UserWalletAccounts>();
            services.AddTransient<IWalletAccountWords, WalletAccountWords>();
            services.AddTransient<IUserWalletAccountTransactions, UserWalletAccountTransactions>();
            services.AddTransient<IProjects, Projects>();
            services.AddTransient<IUserInvestmentProjects, UserInvestmentProjects>();
            services.AddTransient<IProfitabilities, Profitabilities>();
            services.AddTransient<IUserProfiles, UserProfiles>();
            //services.AddTransient<IProfiles, Profiles>();
            //services.AddTransient<IActivities, Activities>();
            //services.AddTransient<IPermissions, Permissions>();
            //services.AddTransient<IRoutes, Routes>();
            services.AddSingleton<FluentValidation.IValidator<User>, UserValidate>();
            services.AddControllers();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                        new CultureInfo("es-ES")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "es-ES", uiCulture: "es-ES");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLocalization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Innovación V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
