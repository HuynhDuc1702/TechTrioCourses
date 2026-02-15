using System;
using System.Collections.Generic;
using CategoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Infrastructure.Data;

public partial class CategoryDbContext : DbContext
{
    public CategoryDbContext()
    {
    }

    public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enable PostgreSQL UUID extension
        modelBuilder.HasPostgresExtension("uuid-ossp");

        // Apply all IEntityTypeConfiguration implementations from this assembly
        // This will automatically find and apply CategoryConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
