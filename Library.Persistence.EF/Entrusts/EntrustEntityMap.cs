using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EF.Entrusts
{
    public class EntrustEntityMap : IEntityTypeConfiguration<Entrust>
    {
        public void Configure(EntityTypeBuilder<Entrust> builder)
        {
            builder.ToTable("Entrusts");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.DeterminateReturnDate).IsRequired(true);
            builder.Property(_ => _.RealReturnDate);

            builder.HasOne(_ => _.Book)
                .WithOne(_ => _.Entrust)
                .HasForeignKey<Entrust>(_ => _.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(_ => _.Member)
                .WithOne(_ => _.Entrust)
                .HasForeignKey<Entrust>(_ => _.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
