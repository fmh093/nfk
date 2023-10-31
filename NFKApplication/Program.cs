using Dapper;
using NFKApplication.Database;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using NFKApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace NFKApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages(options => 
            {
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlite(PathHelper.DatabaseConnectionString)
                .EnableSensitiveDataLogging()
            );

            Log.Logger = new LoggerConfiguration()
                .WriteTo.SQLite(PathHelper.DatabasePath)
                .CreateLogger();
            builder.Logging.AddSerilog();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "https://localhost:7282/",
                        ValidAudience = "https://localhost:7282/",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BALAJFMCAOSPDJA198VNAOCP91AVZOLB1PPANDl1NAV99KL"))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey("auth"))
                            {
                                context.Token = context.Request.Cookies["auth"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin()
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            InitializeDatabase();

            app.Run();


        }

        private static void InitializeDatabase()
        {
            // Comment the line below to reset the data in the database. Don't forget to re-add it once you've reset it.
            //if (IsDatabaseInitialized()) return;

            using var connection = new SQLiteConnection(PathHelper.DatabaseConnectionString);
            connection.Open();


            var initializeQuery = $@"                        
                DELETE FROM Products;                 
                DELETE FROM Configuration;
                DELETE FROM Baskets;
                DELETE FROM Logs;
                VACUUM;

                INSERT INTO Products (Sku, Name, Price, ImagePath)
                VALUES 
                ('7881928', 'Wolf', 29.99, '7881928-p.png'),
                ('7881931', 'Elegant', 39.99, '7881931-p.png'),
                ('7881944', 'Safe', 19.99, '7881944-p.png'),
                ('{PathHelper.SecretMatSku}', 'Secret Mat', 199.99, '7881955-p.png');

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