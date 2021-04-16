using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EF.BookCategories
{
    public class BookCategoryEntityMap :IEntityTypeConfiguration<BookCategory>
    {
        public void Configure(EntityTypeBuilder<BookCategory> builder)
        {
            builder.ToTable("BookCategories");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Title).IsRequired(true).HasMaxLength(50);
        }
    }
}
