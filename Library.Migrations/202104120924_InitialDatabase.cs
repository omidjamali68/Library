using System;
using System.Data;
using FluentMigrator;

namespace Library.Migrations
{
    [Migration(202104141349)]
    public class _202104141349_InitialDatabase : Migration
    {
        public override void Up()
        {
            Create.Table("BookCategories")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("Title").AsString(100).NotNullable();

            Create.Table("Books")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Author").AsString(150)
                .WithColumn("MinAgeNeed").AsInt16().NotNullable()
                .WithColumn("BookCategoryId").AsInt32().NotNullable()
                .ForeignKey("BookCategories", "Id")
                .OnDelete(Rule.Cascade);

            Create.Table("Members")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("FullName").AsString(200).NotNullable()
                .WithColumn("Age").AsInt16().NotNullable()
                .WithColumn("Address").AsString(200);

            Create.Table("Entrusts")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("DeterminateReturnDate").AsDateTime().NotNullable()
                .WithColumn("RealReturnDate").AsDateTime().Nullable()
                .WithColumn("BookId").AsInt32().NotNullable()
                .ForeignKey("FK_Entrusts_Books_Id","Books", "Id")
                .OnDelete(Rule.Cascade)
                .WithColumn("MemberId").AsInt32().NotNullable()
                .ForeignKey("FK_Entrusts_Members_Id","Members", "Id")
                .OnDelete(Rule.Cascade);

        }

        public override void Down()
        {
            Delete.Table("Entrusts");
            Delete.Table("Members");
            Delete.Table("Books");
            Delete.Table("BookCategories");
        }
    }
}
