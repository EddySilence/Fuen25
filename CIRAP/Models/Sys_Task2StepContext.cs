using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class Sys_Task2StepContext : DbContext
    {

        public Sys_Task2StepContext(DbContextOptions<Sys_Task2StepContext> options) : base(options)
        {
        }
        public Sys_Task2StepContext()
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

        public DbSet<Sys_Task2Step> Sys_Task2Step { get; set; }
    }
}
