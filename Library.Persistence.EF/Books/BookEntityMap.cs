using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EF.Books
{
    public class BookEntityMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Name).IsRequired(true).HasMaxLength(150);
            builder.Property(_ => _.Author).IsRequired(true).HasMaxLength(100);
            builder.Property(_ => _.MinAgeNeed).IsRequired(true);
            builder.Property(_ => _.BookCategoryId).IsRequired(true);

            builder.HasOne(_ => _.BookCategory)
                .WithMany(_ => _.Books)
                .HasForeignKey(_ => _.BookCategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
