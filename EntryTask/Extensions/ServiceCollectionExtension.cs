using EntryTask.Application.Interfaces.Logging;
using EntryTask.Application.Services.Logger;
using EntryTask.Infrastructure.Options;
using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using EntryTask.Application.Mapping;

namespace EntryTask.WebAPI.Extensions
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
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(currentAssemblies);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(currentAssemblies));

        }

        public static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            services.AddHttpClient();

            services.Configure<ConnectionStringsOptions>(configuration.GetSection(ConnectionStringsOptions.SectionName));


            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var dbOptions = provider.GetRequiredService<IOptionsSnapshot<ConnectionStringsOptions>>().Value;
                options.UseMySql(dbOptions.DefaultConnection, ServerVersion.Parse(dbOptions.ServerVersion));
            });

            services.AddScoped<ApplicationDbContext>();
            services.AddLogging();
            services.AddControllers();
        }

        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
                opt.CustomSchemaIds(x => x.FullName);
            });
        }
    }
}
