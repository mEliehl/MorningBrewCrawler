using DocoptNet;
using System.Linq;

namespace SqlServer.Migration.Fluent
{
    public class MainArgs
    {
        public const string usage = @"fluentMigrator - A tool to run fluent migrator scripts
            Usage:
                fluentMigrator up [<version>]
                fluentMigrator down <version>
                fluentMigrator rollback <steps>
                fluentMigrator [options]
            Options:
                -h --help     Show this screen.
                --version     Show version.
            ";
        public MainArgs(string[] argsv)
        {
            argsv = argsv.Any() ? argsv : new string[] { "up" };

            var version = typeof(MainArgs).Assembly.GetName().Version.ToString();
            var args = new Docopt().Apply(usage, argsv, version: version);

            if (args["up"].IsTrue)
            {
                long up = 0;
                if (args["<version>"] != null)
                    long.TryParse(args["<version>"].ToString(), out up);
                Up = up;
            }

            if (args["down"].IsTrue)
            {
                long.TryParse(args["<version>"].ToString(), out long down);
                Down = down;
            }

            if (args["rollback"].IsTrue)
                Rollback = args["<steps>"].AsInt;
        }
        public long? Up { get; }
        public long? Down { get; }
        public int? Rollback { get; }
    }
}