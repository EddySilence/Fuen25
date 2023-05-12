using Microsoft.EntityFrameworkCore;


namespace CIRAP.Models
{
    public class sys_SubProjectContext : DbContext
    {

        public sys_SubProjectContext(DbContextOptions<sys_SubProjectContext> options) : base(options)
        {
        }
        public sys_SubProjectContext()
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

        public DbSet<sys_SubProject> sys_SubProject { get; set; }
    }
}
