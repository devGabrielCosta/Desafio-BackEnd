using Domain;
using Infrastructure;
using System.Text.Json.Serialization;

namespace Application.Configuration
{
    public static class DependecyInjectionExtension
    {
        public static void ConfigureDepencyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
                logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
            });

            services.AddJwtAuthentication(configuration);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithJWTAuth();

            services.AddPSQLContext(configuration);
            services.AddRepositories();

            services.AddServices();
            services.AddCommandHandlers();

            services.AddRabbitMq();
            services.AddAmazonStorage();
        }
    }
}
