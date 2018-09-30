using FluentMigrator;
using System;
using System.IO;

namespace SqlServer.Migration.Fluent.Migrations
{
    [Migration(20180808205000)]
    public class CreateAuthorsTable : FluentMigrator.Migration
    {
        public override void Up()
        {
            var script = Path.Combine(AppContext.BaseDirectory, "Scripts", "CreateAuthorTableUp.sql");
            Execute.Script(script);
        }

        public override void Down()
        {
            var script = Path.Combine(AppContext.BaseDirectory, "Scripts", "CreateAuthorTableDown.sql");
            Execute.Script(script);
        }
    }
}