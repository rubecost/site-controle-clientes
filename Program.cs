
using ControleClientesMvc.ApiClients;
using ControleClientesMvc.Services;

namespace ControleClientesMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AutenticacaoService>();
            builder.Services.AddScoped<VerificarValidadeJwt>();
            builder.Services.AddControllersWithViews();

            // Adiciona serviços de sessão
            builder.Services.AddDistributedMemoryCache(); 
            builder.Services.AddSession(options =>
            {
                // Encerra sessão em 30min de inatividade
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true; 
            });

            var app = builder.Build();

            // the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Usuarios}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
