using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        { /* Fluent Mapping */

            // [Table("Category")]
            builder.ToTable("Category");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(); // IDENTITY (1, 1)

            // Properties
            builder.Property(x => x.Name)
                .IsRequired() // NOT NULL
                .HasColumnName("Name") // apenas se o nome da prop da classe for diferente da coluna da tabela
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired() // NOT NULL
                .HasColumnName("Slug") // apenas se o nome da prop da classe for diferente da coluna da tabela
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            // Índices
            builder.HasIndex(x => x.Slug, "IX_Category_Slug")
                .IsUnique();
        }
    }
}