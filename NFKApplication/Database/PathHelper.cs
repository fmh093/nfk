namespace NFKApplication.Database
{
    public static class PathHelper
    {
        public static string DatabaseConnectionString => @"Data Source=" + Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\nfk.db"));        
        public static string ImagesPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Images\\");        
    }
}
