
using FluentMigrator;

namespace SqlServer.Migration.Fluent.Migrations
{
    [Migration(20180806131500)]
    public class CreateArticleTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("Article")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Date").AsDateTime()
                .WithColumn("Link").AsString(int.MaxValue)
                .WithColumn("Title").AsString(int.MaxValue)
                .WithColumn("Authors").AsString(int.MaxValue);
        }

        public override void Down()
        {
            Delete.Table("Article");
        }
    }
}