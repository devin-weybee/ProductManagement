using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AssignedProduct> AssignedProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<AssignedProduct>().ToTable("AssignedProduct");
        }

        public List<Customer> sp_GetAllCustomers()
        {
            return Customers.FromSqlRaw("EXECUTE [dbo].GetAllCustomers").ToList();
        }
    }
}
