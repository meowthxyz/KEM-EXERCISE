using Microsoft.EntityFrameworkCore;

namespace KEM_WPF.Data
{
    public  class KEMDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<RegisteredEquipment> RegisteredEquipments { get; set; }

        public KEMDbContext()
            : this(new DbContextOptionsBuilder<KEMDbContext>().UseSqlServer("Data Source=ACER\\SQLEXPRESS;Initial Catalog=KEM_DB;Integrated Security=True;TrustServerCertificate=True").Options)
        { }

        public KEMDbContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("user");
            builder.Entity<Equipment>().ToTable("equipment");
            builder.Entity<Site>().ToTable("site");
            builder.Entity<RegisteredEquipment>().ToTable("registered_equipment");
        }
    }
}
