using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RentalManagement.Models;

using System;

namespace RentalManagement.Data

{


    public class FlicksDbContext : DbContext
    {
        public FlicksDbContext(DbContextOptions<FlicksDbContext> options): base(options) { }

        
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Person>().ToTable("Persons").HasKey(p => p.UserID);
            modelBuilder.Entity<Movie>().ToTable("Movies").HasKey(m => m.MovieID);

            
            modelBuilder.Entity<Person>().Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<Movie>().Metadata.SetIsTableExcludedFromMigrations(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}



