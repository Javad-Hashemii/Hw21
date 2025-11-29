using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        }
    }
}
