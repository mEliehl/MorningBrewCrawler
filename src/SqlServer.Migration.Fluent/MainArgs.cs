namespace SqlServer.Migration.Fluent
{
    public class MainArgs
    {
        public MainArgs(string[] args)
        {
            
        }
        public long Up { get; }
        public long Down { get; }

        public long Rollback { get; }
    }
}