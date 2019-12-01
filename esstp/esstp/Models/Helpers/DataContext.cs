using Microsoft.EntityFrameworkCore;
using esstp.Models;

namespace esstp
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        //public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Cfd> Cfds { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder
        //Log parameter values
        .EnableSensitiveDataLogging();
    }
}
