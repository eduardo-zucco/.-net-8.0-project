using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;

namespace SalesWebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Obtém a connection string do appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext")
                ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.");

            // ✅ Configuração do MySQL com Pomelo
            builder.Services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly("SalesWebMvc");
                    }));

            // ✅ Adiciona MVC (controllers + views)
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ✅ Pipeline de requisições HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
