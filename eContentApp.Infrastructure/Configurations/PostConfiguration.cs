using eContentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eContentApp.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Thumnail_url)
                .HasMaxLength(500);

            builder.Property(p => p.PublishedAt)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();
        }
    }
}
