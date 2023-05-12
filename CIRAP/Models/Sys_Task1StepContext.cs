using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class Sys_Task1StepContext : DbContext
    {

        public Sys_Task1StepContext(DbContextOptions<Sys_Task1StepContext> options) : base(options)
        {
        }
        public Sys_Task1StepContext()
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
                //optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-BENSON\SQLEXPRESS;Initial Catalog=CIRAP;Integrated Security=True;TrustServerCertificate=true;");
            }
        }

        public DbSet<Sys_Task1Step> Sys_Task1Step { get; set; }
    }
}
