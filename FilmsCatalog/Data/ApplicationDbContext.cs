using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FilmsCatalog.Models;

namespace FilmsCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public ApplicationDbContext()
        //{
        //    Database.EnsureCreated();
        //}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(new Category { Id = 1, Name = "Horror" });
            builder.Entity<Category>().HasData(new Category { Id = 2, Name = "Comedy" });

        }
    }
}
