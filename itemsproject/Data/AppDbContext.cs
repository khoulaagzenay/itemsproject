using Microsoft.EntityFrameworkCore;
using itemsproject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace itemsproject.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Select category" },
                new Category() { Id = 2, Name = "It" },
                new Category() { Id = 3, Name = "Mobiles" }
            );

 
            modelBuilder.Entity<Client>()
        .HasMany(c => c.Items)
        .WithMany(i => i.Clients)
        .UsingEntity<Dictionary<string, object>>(
            "ClientItem", // actual join table name in DB
            j => j.HasOne<Item>().WithMany().HasForeignKey("ItemId"),   // column name in DB
            j => j.HasOne<Client>().WithMany().HasForeignKey("ClientId")); // column name in DB


            base.OnModelCreating(modelBuilder);
        }
    }
}
