using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Blog.Data;
using Blog.Extensions;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureMvc(builder);
// ConfigureServices(builder);
builder.LoadConfiguration();
builder.ConfigureServices();

builder.Services.AddEndpointsApiExplorer(); // Adiciona o Swagger
builder.Services.AddSwaggerGen(); // Gera o HTML do Swagger

var app = builder.Build();

// LoadConfiguration(app);

// HTTPS -> conexão estará encriptada -> necessita de um certificado digital
/*
    - é possível comprar um certificado digital
    - azure disponibiliza um gratuito 
    - diferença do gratuito para o pago seria que o pago tem seguro e o gratuito não
*/


// colocar primeiro, antes dos outros, exceto da documentação do swagger
// app.UseHttpsRedirection(); // --> redireciona para HTTPS automáticao mesmo se alguém fizer uma chamada sem o https - NÃO PERMITE REQUISIÇÕES HTTP

// Sempre nessa ordem: autenticação antes da autorização
app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseStaticFiles(); // Serve para o servidor conseguir renderizar imagem, css, js e etc

app.MapControllers();

/*
    Caso queira que um pedaço do códio exclusivamente se estiver em desenvolvimento por exemplo


    - IsProduction -> Produção
    - IsEnvironment -> Ambientes personalizados
    - IsStaging = -> Ambiente QAS
*/
if (app.Environment.IsDevelopment())
{
    // Adicionar Documentação do swagger apenas se a API estiver em modo de desenvolvimento - dependendo da API pode até deixar visível em produção
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

void LoadConfiguration(WebApplication app)
{
    // app.Configuration.GetSection("Smtp); // pega uma chave do json no appsettings
    // app.Configuration.GetValue<string>("JwtKey"); // pega uma chave do json no appsettings

    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder
        .Services
        .AddAuthentication(x =>
        {
            /* configura qual esquema de autenticação iremos utilizar */
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Também é mais útil para autenticar uma múltiplas API's
        }).AddJwtBearer(x => // informar como vamos desencriptar este token
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                // As duas opções abaixo seriam úteis para autenticar múltiplas API's
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(options =>
    {
        // options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>(); // uma das mais populares
        // options.Providers.Add<CustomCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });
    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(x =>
        {
            // Ignora ciclos subsequentes da serialização dos objetos. Ex: Post tem uma categoria e categoria tem um post, com essa opção não terá um loop, vai chegar no primeiro nó e vai ignorar o ciclo
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            // Essa opção abaixo habilita a remoção de uma propriedade se ele tiver valor nulo
            // x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
}

void ConfigureServices(WebApplicationBuilder builder)
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