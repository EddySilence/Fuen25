using Microsoft.EntityFrameworkCore;
using CIRAP.ViewModels;


namespace CIRAP.Models
{
    public class UserContext : DbContext
    {

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public UserContext()
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

        public DbSet<User> User { get; set; }

        public DbSet<CIRAP.ViewModels.RegisterVierwModel>? RegisterVierwModel { get; set; }

        public DbSet<CIRAP.ViewModels.verifyViewModel>? verifyViewModel { get; set; }
    }
}
