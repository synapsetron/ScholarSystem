using EntryTask.WebAPI.Extensions;

using Serilog;
namespace EntryTask
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


      
            builder.Configuration.AddUserSecrets<Program>();
        

            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddSwaggerServices();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddCustomServices();
            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EntryTask API v1");
                });
            }
            await app.ApplyMigrations();


            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}