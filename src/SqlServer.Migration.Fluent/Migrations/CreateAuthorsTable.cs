using System.IO;
using FluentMigrator;

namespace SqlServer.Migration.Fluent.Migrations
{
    [Migration(20180808205000)]
    public class CreateAuthorsTable : FluentMigrator.Migration
    {
        public override void Up()
        {            
            var script = Path.Combine("Scripts","CreateAuthorTableUp.sql");
            Execute.Script(script);            
        }

        public override void Down()
        {
            var script = Path.Combine("Scripts","CreateAuthorTableDown.sql");
            Execute.Script(script);
        }
    }
}