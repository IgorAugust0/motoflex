using Motoflex.Infrastructure;
using Motoflex.Application;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Motoflex.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        private static readonly string[] OrderedGroups = ["motos", "entregadores", "locacao"];

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Logging
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
                logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
            });

            // Configure JWT Authentication
            // services.AddOptions<JwtOptions>()
            //     .Bind(configuration.GetSection(JwtOptions.SectionName))
            //     .ValidateDataAnnotations()
            //     .ValidateOnStart();
            
            // services.AddJwtAuthentication(configuration);

            // Configure Controllers and JSON Options
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            services.AddEndpointsApiExplorer();
            // TODO: Configure Swagger Gen with JWT auth
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Motoflex API", Version = "v1" });

                // Get the current assembly
                var assembly = Assembly.GetExecutingAssembly();
                // Get the XML file path that matches your assembly name
                var xmlFile = $"{Path.GetFileNameWithoutExtension(assembly.Location)}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // TODO: fix sorting of groups
                c.OrderActionsBy(apiDesc => Array.IndexOf(OrderedGroups, apiDesc.GroupName).ToString("D2"));
            });

            // Configure Database Context and Repositories
            services.AddPSQLContext(configuration);
            services.AddRepositories();

            // Configure Application Services and *Command Handlers
            services.AddServices();

            // TODO: Configure *Messaging and Storage
            //services.AddRabbitMq();
            services.AddAmazonStorage();
        }
    }
}
