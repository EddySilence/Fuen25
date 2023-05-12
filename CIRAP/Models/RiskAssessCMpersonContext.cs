using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class RiskAssessCMpersonContext : DbContext
    {

        public RiskAssessCMpersonContext(DbContextOptions<RiskAssessCMpersonContext> options) : base(options)
        {
        }
        public RiskAssessCMpersonContext()
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

        public DbSet<RiskAssessCMperson> RiskAssessCMperson { get; set; }
    }
}
