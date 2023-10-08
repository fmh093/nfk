using Dapper;
using NFKApplication.Database;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;

namespace NFKApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(PathHelper.DatabaseConnectionString));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                // Add more route mappings for other controllers as needed
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            InitializeDatabase();

            app.Run();


        }

        private static void InitializeDatabase()
        {
            // Comment the line below to reset the data in the database. Don't forget to re-add it once you've reset it.
            if (IsDatabaseInitialized()) return;

            using var connection = new SQLiteConnection(PathHelper.DatabaseConnectionString);
            connection.Open();


            var initializeQuery = @"                        
                DELETE FROM Products;                 
                DELETE FROM Configuration;
                DELETE FROM Baskets;
                VACUUM;

                INSERT INTO Products (Sku, Name, Price, ImagePath)
                VALUES 
                ('7881928', 'Wolf', 29.99, '7881928-p.png'),
                ('7881931', 'Elegant', 39.99, '7881931-p.png'),
                ('7881944', 'Safe', 19.99, '7881944-p.png');

                INSERT INTO Configuration (Initialized) VALUES (1);";

            using var cmd = new SQLiteCommand(initializeQuery, connection);
            cmd.ExecuteNonQuery();
        }

        private static bool IsDatabaseInitialized()
        {
            using var connection = new SQLiteConnection(PathHelper.DatabaseConnectionString);
            connection.Open();
            var initialized = connection.Query<bool>("SELECT Initialized FROM Configuration WHERE Initialized = 1").FirstOrDefault();
            return initialized;
        }
    }
}