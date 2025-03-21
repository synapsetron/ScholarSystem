﻿using ScholarSystem.Application.Interfaces.Logging;
using ScholarSystem.Application.Services.Logger;
using ScholarSystem.Infrastructure.Options;
using ScholarSystem.Infrastructure.Persistence;
using ScholarSystem.Infrastructure.Repositories.Interfaces.Base;
using ScholarSystem.Infrastructure.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ScholarSystem.Application.Mapping;
using Microsoft.AspNetCore.Identity;
using ScholarSystem.Domain.Entities;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ScholarSystem.Application.Interfaces.User;
using ScholarSystem.Application.Services.User.JWT;
using ScholarSystem.Application.Services.User;
using ScholarSystem.Application;
using FluentValidation.AspNetCore;
using ScholarSystem.WebAPI.Middleware;

namespace ScholarSystem.WebAPI.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddRepositoryServices();
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserService,  UserService>();
            
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(currentAssemblies);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(currentAssemblies));

        }

        public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            services.AddHttpClient();

            services.Configure<ConnectionStringsOptions>(configuration.GetSection(ConnectionStringsOptions.SectionName));
            services.Configure<JwtSettingsOptions>(configuration.GetSection(JwtSettingsOptions.SectionName));

            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var dbOptions = provider.GetRequiredService<IOptionsSnapshot<ConnectionStringsOptions>>().Value;
                options.UseMySql(dbOptions.DefaultConnection, ServerVersion.Parse(dbOptions.ServerVersion));
            });

            services.AddScoped<ApplicationDbContext>();

            services.AddApplicationServices();
            services.AddFluentValidationAutoValidation();
            services.AddLogging();
            services.AddControllers();
        }
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>( o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

        }
        public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtSettingsOptions>()
                .Bind(configuration.GetSection(JwtSettingsOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<IValidateOptions<JwtSettingsOptions>, ValidateJwtSettingsOptions>();

            // take a settings from jwt
            var jwtSettings = configuration.GetSection(JwtSettingsOptions.SectionName).Get<JwtSettingsOptions>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
            {
                throw new InvalidOperationException("JWT Secret key is missing in configuration.");
            }

            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();
        }

        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
                opt.CustomSchemaIds(x => x.FullName);

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token in the following format: {your token here} do not add the word 'Bearer' before it."
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
