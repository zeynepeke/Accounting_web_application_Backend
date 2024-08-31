using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Veritabanı bağlantı dizesi alınıyor
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // DbContext yapılandırılıyor
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            );

            // Servisler ekleniyor
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<OrderService>();

            // CORS policy yapılandırması
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Diğer servisler
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Geliştirme ortamında Swagger UI kullanılıyor
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // HTTPS yönlendirme ve CORS policy kullanımı
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            // API Controller'lar
            app.MapControllers();

            // Uygulama çalıştırılıyor
            app.Run();
        }
    }
}
