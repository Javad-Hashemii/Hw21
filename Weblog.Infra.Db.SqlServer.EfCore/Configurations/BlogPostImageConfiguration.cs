using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Infra.Db.SqlServer.EfCore.Configurations
{
    public class BlogPostImageConfiguration : IEntityTypeConfiguration<BlogPostImage>
    {
        public void Configure(EntityTypeBuilder<BlogPostImage> builder)
        {

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ImagePath)
                .IsRequired()
                .HasMaxLength(500);

            // Relationship: One Post -> Many Images
            builder.HasOne(i => i.BlogPost)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.BlogPostId);

            builder.HasData(
                new BlogPostImage
                {
                    Id = 1,
                    BlogPostId = 1,
                    ImagePath = "https://cdn.example.com/posts/welcome.jpg"
                },
                new BlogPostImage
                {
                    Id = 2,
                    BlogPostId = 2,
                    ImagePath = "https://cdn.example.com/posts/why-csharp-is-awesome.jpg"
                }
            );
        }
    }
}
