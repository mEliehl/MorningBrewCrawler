using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DbUp;

namespace SqlServer.Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString =
                args.FirstOrDefault()
                ?? "Data Source=localhost;Initial Catalog=MorningBrew;Persist Security Info=True;User ID=sa;Password=Crawler_Brew";

            EnsureDatabase.For.SqlDatabase(connectionString);
            
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();            
        }
    }
}
