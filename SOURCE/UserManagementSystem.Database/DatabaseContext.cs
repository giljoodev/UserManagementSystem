using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagementSystem.Database.Entity;

namespace UserManagementSystem.Database
{
    public class DatabaseContext : DbContext
    {
        public List<User> Users { get; set; } = new List<User>();
        public virtual DbSet<User> User { get; set; }

        public DatabaseContext()
        {
            //try
            //{
            //    if (Database.CanConnect() == true)
            //    {
            //        return;
            //    }

            //    Database.Migrate();
            //}
            //catch
            //{
            //    throw;
            //}
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            string connectionString = config.GetRequiredSection("ConnectionStrings:UserManagementSystem").Get<string>();
            optionsBuilder.UseSqlServer(connectionString);
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserManagementSystem;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Users = new List<User>();

            modelBuilder.Entity<User>().Property(p => p.Index).ValueGeneratedOnAdd();

            Users.Add(new User() { Index = 1,  Name = "적길동", Age = 10, PhoneNumber = "01012341200", IsInit = true });
            Users.Add(new User() { Index = 2,  Name = "홍길동", Age = 15, PhoneNumber = "01012341201", IsInit = true });
            Users.Add(new User() { Index = 3,  Name = "황길동", Age = 20, PhoneNumber = "01012341202", IsInit = true });
            Users.Add(new User() { Index = 4,  Name = "록길동", Age = 25, PhoneNumber = "01012341203", IsInit = true });
            Users.Add(new User() { Index = 5,  Name = "청길동", Age = 30, PhoneNumber = "01012341204", IsInit = true });
            Users.Add(new User() { Index = 6,  Name = "남길동", Age = 35, PhoneNumber = "01012341205", IsInit = true });
            Users.Add(new User() { Index = 7,  Name = "자길동", Age = 40, PhoneNumber = "01012341206", IsInit = true });
            Users.Add(new User() { Index = 8,  Name = "백길동", Age = 45, PhoneNumber = "01012341207", IsInit = true });
            Users.Add(new User() { Index = 9,  Name = "회길동", Age = 50, PhoneNumber = "01012341208", IsInit = true });
            Users.Add(new User() { Index = 10, Name = "흑길동", Age = 55, PhoneNumber = "01012341209", IsInit = true });

            foreach (var user in Users)
            {
                modelBuilder.Entity<User>().HasData(user);
            }
        }
    }
}
