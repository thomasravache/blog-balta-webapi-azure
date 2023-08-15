using Blog.Data;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

namespace Blog.Extensions
{
    public static class AppExtensions
    {
        public static void LoadConfiguration(this WebApplicationBuilder builder)
        {
            // app.Configuration.GetSection("Smtp); // pega uma chave do json no appsettings
            // app.Configuration.GetValue<string>("JwtKey"); // pega uma chave do json no appsettings

            Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
            Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
            Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");

            var smtp = new Configuration.SmtpConfiguration();
            builder.Configuration.GetSection("Smtp").Bind(smtp);
            Configuration.Smtp = smtp;
        }

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<BlogDataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }); // para dataContext sempre utilizar o AddDbContext

            /*
                Diferença do Transient, Scoped e Singleton é o tempo de vida da dependência

                AddTransient -->
                    Sempre cria uma nova instancia quando você precisar (instanciar)

                AddScoped -->
                    Dura por transação sempre que fizer um "request" ele reaproveita a instância
                    naquela requisição, depois que finalizar a requisição ele mata o serviço solicitado

                AddSingleton -->
                    Uma instância por App, ou seja, depois que a aplicação subir ele vai carregar uma vez só
                    e vai ficar lá na memória do app pra sempre
            */

            // builder.Services.AddScoped();
            // builder.Services.AddSingleton();
            builder.Services.AddTransient<TokenService>();
            builder.Services.AddTransient<EmailService>();
        }
    }
}