using ScholarSystem.WebAPI.Extensions;

using Serilog;
namespace ScholarSystem
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
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddAuthServices(builder.Configuration);
            builder.Services.AddCorsPolicies();
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ScholarSystem API v1");
                });
            }
            await app.ApplyMigrations();

            app.UseCors("AllowAllOrigins");
            app.MapControllers();
            app.Run();
        }
    }
}