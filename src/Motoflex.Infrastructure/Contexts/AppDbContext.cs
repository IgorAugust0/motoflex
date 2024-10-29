using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Motoflex.Infrastructure.Contexts
{
    /// <summary>
    /// Fluent API based configurations for the database context
    /// </summary>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
