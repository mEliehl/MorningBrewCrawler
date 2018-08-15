using System.Data;
using System.IO;
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
            
            var script = Path.Combine("Scripts","AddAuthorTableUp.sql");
            Execute.Script(script);

            Delete.Column("Authors").FromTable("Article");
        }

        public override void Down()
        {
            Alter.Table("Article").AddColumn("Authors").AsString(int.MaxValue).WithDefaultValue(string.Empty);
            var script = Path.Combine("Scripts","AddAuthorTableDown.sql");
            Execute.Script(script);
            Delete.Table("Author");
        }
    }
}