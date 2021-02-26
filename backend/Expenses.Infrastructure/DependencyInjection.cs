using Expenses.Application.Common.Interfaces;
using Expenses.Infrastructure.Identity;
using Expenses.Infrastructure.Persistence;
using Expenses.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Expenses.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // https://github.com/microsoft/FeatureManagement-Dotnet
            services.AddFeatureManagement();
            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<AppDbContext>(options =>
            //        options.UseInMemoryDatabase("CleanArchitectureDb"));
            //}
            //else
            //{
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("Test")
                        ,
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                        );
            //}

            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, AppDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = configuration["JwtToken:Issuer"],
            //            ValidAudience = configuration["JwtToken:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"])),
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }
    }
}
