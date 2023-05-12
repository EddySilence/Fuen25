using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class RiskAssessContext : DbContext
    {

        public RiskAssessContext(DbContextOptions<RiskAssessContext> options) : base(options)
        {
        }
        public RiskAssessContext()
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

        public DbSet<RiskAssess> RiskAssess { get; set; }
    }
}
