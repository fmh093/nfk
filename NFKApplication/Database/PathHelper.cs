namespace NFKApplication.Database
{
    public static class PathHelper
    {
        public static string DatabaseConnectionString => @"Data Source=" + DatabasePath;
        public static string DatabasePath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\nfk.db"));
        public static string ImagesPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\wwwroot\\images\\"));
        public static string SecretMatSku = "7881955";
    }
}
