using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class ProjectTasksContext : DbContext
    {
        public ProjectTasksContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration config = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<ProjectTasks> ProjectTasks { get; set; }
    }
}
