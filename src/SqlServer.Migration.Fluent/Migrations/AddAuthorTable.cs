using System.Data;
using FluentMigrator;

namespace SqlServer.Migration.Fluent.Migrations
{
    [Migration(20180808205000)]
    public class CreateAuthorsTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("Author")
                .WithColumn("ArticleId").AsGuid().ForeignKey("Article","Id").OnDelete(Rule.Cascade)
                .WithColumn("Name").AsString(int.MaxValue);
            
            Execute.Script("Scripts\\AddAuthorTableUp.sql");

            Delete.Column("Authors").FromTable("Article");
        }

        public override void Down()
        {
            Alter.Table("Article").AddColumn("Authors").AsString(int.MaxValue).WithDefaultValue(string.Empty);
            Execute.Script("Scripts\\AddAuthorTableDown.sql");
            Delete.Table("Author");
        }
    }
}