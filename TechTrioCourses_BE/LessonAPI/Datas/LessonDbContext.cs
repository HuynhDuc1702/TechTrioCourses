using System;
using System.Collections.Generic;
using TechTrioCourses.Shared.Enums;
using LessonAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonAPI.Datas;

public partial class LessonDbContext : DbContext
{
    public LessonDbContext()
    {
    }

    public LessonDbContext(DbContextOptions<LessonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lesson> Lessons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lessons_pkey");

            entity.ToTable("lessons");

            entity.HasIndex(e => e.CourseId, "idx_lessons_course");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");
            entity.Property(e => e.MediaType)
               .HasConversion<short>()
 .HasColumnName("media_type");
            entity.Property(e => e.MediaUrl).HasColumnName("media_url");
            entity.Property(e => e.OrderIndex)
                .HasDefaultValue(0)
                .HasColumnName("order_index");
            entity.Property(e => e.Status)
                  .HasConversion<short>()
 .HasDefaultValue(PublishStatusEnum.Hidden)
 .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
