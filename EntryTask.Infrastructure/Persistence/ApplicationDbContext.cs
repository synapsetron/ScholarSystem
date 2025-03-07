﻿using Microsoft.EntityFrameworkCore;
using EntryTask.Domain.Entities;

namespace EntryTask.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
            public DbSet<Student> Students { get; set; } = null!;
            public DbSet<Teacher> Teachers { get; set; } = null!;
            public DbSet<Course> Courses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
