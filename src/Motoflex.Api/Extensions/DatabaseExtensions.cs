using Microsoft.EntityFrameworkCore;
using Motoflex.Infrastructure.Contexts;

namespace Motoflex.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task MigrateDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                logger.LogInformation("Starting database migration...");

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Found {count} pending migrations", pendingMigrations.Count());
                    foreach (var migration in pendingMigrations)
                    {
                        logger.LogInformation("Pending migration: {migration}", migration);
                    }

                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migration completed successfully");
                }
                else
                {
                    logger.LogInformation("No pending migrations found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database");
                throw;
            }
        }
    }
}
