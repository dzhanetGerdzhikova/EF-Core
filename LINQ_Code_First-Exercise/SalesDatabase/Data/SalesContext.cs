using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace 	P03_SalesDatabase.Data
{
  public  class SalesContext:DbContext
    {
        public SalesContext()
        {

        }
        public SalesContext(DbContextOptions<SalesContext> options)
            :base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Sales;Integrated Security=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.Property(p => p.Name).IsUnicode(true);
                product.Property(p => p.Description).HasDefaultValue("No description");
            });
            modelBuilder.Entity<Customer>(customer =>
            {
                customer.Property(c => c.Name).IsUnicode(true);
                customer.Property(c => c.Email).IsUnicode(false);
            });
            modelBuilder.Entity<Store>(store =>
            {
                store.Property(s => s.Name).IsUnicode(true);
            });
            modelBuilder.Entity<Sale>(sale =>
            {
                sale.Property(s => s.Date).HasDefaultValueSql("GETDATE()");
            });
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
