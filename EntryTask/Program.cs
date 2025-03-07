using EntryTask.Infrastructure.Options;
using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using EntryTask.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EntryTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EntryTask API", Version = "v1" });
            });

            builder.Configuration.AddUserSecrets<Program>();

            builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection(ConnectionStringsOptions.SectionName));
            

            builder.Services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var dbOptions = provider.GetRequiredService<IOptionsSnapshot<ConnectionStringsOptions>>().Value;
                options.UseMySql(dbOptions.DefaultConnection, ServerVersion.Parse(dbOptions.ServerVersion));
            });

            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddControllers();

        

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.ApplyMigrations();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EntryTask API v1");
                });
            }
           

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}