using EntryTask.Infrastructure.Options;
using EntryTask.Infrastructure.Persistence;
using EntryTask.Infrastructure.Repositories.Interfaces.Base;
using EntryTask.Infrastructure.Repositories.Realizations.Base;
using EntryTask.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EntryTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}