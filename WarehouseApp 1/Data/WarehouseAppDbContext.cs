using Microsoft.EntityFrameworkCore;
using WarehouseApp.Entities;
using WarehouseApp.Entities.Extensions;

namespace WarehouseApp.Data
{
    public class WarehouseAppDbContext : DbContext
    {
        public IEnumerable<Equipment> Helmets => Set<Helmet>();
        public IEnumerable<Equipment> Skis => Set<Ski>();
        public IEnumerable<Equipment> Snowboards => Set<Snowboard>();
        public IEnumerable<Equipment> Shoes => Set<Shoe>();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Helmet>();
            modelBuilder.Entity<Ski>();
            modelBuilder.Entity<Snowboard>();
            modelBuilder.Entity<Shoe>();
        }
    }
}