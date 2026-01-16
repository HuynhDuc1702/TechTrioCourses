using System;
using System.Collections.Generic;
using CategoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Datas;

public partial class CategoryContext : DbContext
{
    public CategoryContext()
    {
    }

    public CategoryContext(DbContextOptions<CategoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
