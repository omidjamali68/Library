using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.EF.Members
{
    public class MemberEntityMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.FullName).IsRequired(true).HasMaxLength(150);
            builder.Property(_ => _.Age).IsRequired(true);
            builder.Property(_ => _.Address).IsRequired(true).HasMaxLength(200);
        }
    }
}
