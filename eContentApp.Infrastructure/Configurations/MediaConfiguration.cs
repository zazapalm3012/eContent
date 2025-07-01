using eContentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eContentApp.Infrastructure.Configurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.Property(m => m.FileName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.FilePath)
                .IsRequired();

            builder.Property(m => m.FileType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Caption)
                .HasMaxLength(500);
        }
    }
}
