using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Employee)
                .HasForeignKey<Account>(b => b.NIK);
            modelBuilder.Entity<Account>()
               .HasOne(a => a.Profiling)
               .WithOne(b => b.Account)
               .HasForeignKey<Profiling>(b => b.NIK);
            modelBuilder.Entity<Education>()
               .HasMany(a => a.Profilings)
               .WithOne(b => b.Education);
            modelBuilder.Entity<University>()
              .HasMany(a => a.Education)
              .WithOne(b => b.University);
            modelBuilder.Entity<Role>()
              .HasMany(a => a.AccountRole)
              .WithOne(b => b.Role);
            modelBuilder.Entity<Account>()
              .HasMany(a => a.AccountRole)
              .WithOne(b => b.Account);
        }
     
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts{ get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
    }

        
}
