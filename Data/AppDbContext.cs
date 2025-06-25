using CafeBestelTerminal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Producten { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<BestellingProduct> BestellingProducten { get; set; }

        public AppDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=cafe.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BestellingProduct>()
                .HasOne(bp => bp.Bestelling)
                .WithMany(b => b.BestellingProducten)
                .HasForeignKey(bp => bp.BestellingId);

            modelBuilder.Entity<BestellingProduct>()
                .HasOne(bp => bp.Product)
                .WithMany()
                .HasForeignKey(bp => bp.ProductId);
        }
    }
}
