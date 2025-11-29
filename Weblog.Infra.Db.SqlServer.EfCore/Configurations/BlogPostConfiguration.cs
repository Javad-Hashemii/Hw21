using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Infra.Db.SqlServer.EfCore.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.PublishedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(b=>b.Text)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(b => b.ImageUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(b => b.AuthorId)
                .IsRequired();

            builder.HasOne(b => b.Category)
                .WithMany(c => c.BlogPosts)
                .HasForeignKey(b => b.CategoryId);

            string adminId = "c7f338a6-8bc3-44aa-aa59-d475f5674419";

            builder.HasData(
                 new BlogPost
                 {
                     Id = 1,
                     Title = "Welcome to the Weblog",
                     Text = "This is the first seeded post on the platform. Stay tuned for more updates!",
                     PublishedDate = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
                     ImageUrl = "https://cdn.example.com/posts/welcome.jpg",
                     AuthorId = adminId,
                     CategoryId = 1 // Technology
                 },
                 new BlogPost
                 {
                     Id = 2,
                     Title = "Why C# is Awesome",
                     Text = "C# offers a great balance between performance and developer productivity. LINQ is magic.",
                     PublishedDate = new DateTime(2023, 10, 5, 14, 30, 0, DateTimeKind.Utc),
                     ImageUrl = "https://cdn.example.com/posts/why-csharp-is-awesome.jpg",
                     AuthorId = adminId,
                     CategoryId = 3 // Coding
                 }
             );
        }
    }
}
