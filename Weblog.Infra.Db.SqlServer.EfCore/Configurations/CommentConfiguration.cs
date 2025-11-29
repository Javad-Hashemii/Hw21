using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Infra.Db.SqlServer.EfCore.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Text)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Email)
                .HasMaxLength(100);

            // UserId is optional (nullable) because guests can comment
            builder.Property(c => c.UserId)
                .IsRequired(false);

            builder.Property(c => c.Status)
                .HasConversion<int>()
                .HasDefaultValue(CommentStatus.Pending);

            // Relationship: A Comment belongs to a Post
            builder.HasOne(c => c.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            var adminId = "c7f338a6-8bc3-44aa-aa59-d475f5674419";

            builder.HasData(
                // Comment 1: By a Guest on Post 1
                new Comment
                {
                    Id = 1,
                    BlogPostId = 1,
                    Text = "Great introduction! Looking forward to more.",
                    Name = "Guest Visitor",
                    Email = "visitor@example.com",
                    CreatedAt = new DateTime(2023, 10, 2, 9, 0, 0, DateTimeKind.Utc),
                    Status = CommentStatus.Approved,
                    Rating = 5,
                    UserId = null // Guest
                },
                // Comment 2: By the Admin on Post 2
                new Comment
                {
                    Id = 2,
                    BlogPostId = 2,
                    Text = "I totally agree, LINQ makes life so much easier.",
                    Name = "System Admin",
                    Email = "admin@weblog.com",
                    CreatedAt = new DateTime(2023, 10, 6, 10, 0, 0, DateTimeKind.Utc),
                    Status = CommentStatus.Approved,
                    Rating = 5,
                    UserId = adminId // Linked to Admin User
                }
            );
        }
    }
}
