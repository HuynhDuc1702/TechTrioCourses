using CategoryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CategoryAPI.Infrastructure.Data.Configurations
{
    
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
           
            builder.HasKey(e => e.Id)
                .HasName("categories_pkey");

           
            builder.ToTable("categories");

      
            builder.HasIndex(e => e.Name, "categories_name_key")
                .IsUnique();

           
            builder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("created_at");

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(CURRENT_TIMESTAMP AT TIME ZONE 'UTC'::text)")
                .HasColumnName("updated_at");
        }
    }
}
