using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Infra.Db.SqlServer.EfCore.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.OwnerId)
                .IsRequired();


            string adminId= "c7f338a6-8bc3-44aa-aa59-d475f5674419";

            builder.HasData(
                new Category { Id = 1, Name = "Technology", OwnerId = adminId },
                new Category { Id = 2, Name = "Lifestyle", OwnerId = adminId },
                new Category { Id = 3, Name = "Coding", OwnerId = adminId }
            );

        }
    }
}
