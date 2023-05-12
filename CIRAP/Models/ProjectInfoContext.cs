using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class ProjectInfoContext : DbContext
    {

        public ProjectInfoContext(DbContextOptions<ProjectInfoContext> options) : base(options)
        {
        }
        public ProjectInfoContext()
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

        public DbSet<ProjectInfo> ProjectInfo { get; set; }
    }
}
