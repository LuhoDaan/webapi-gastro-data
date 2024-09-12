using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;
using Npgsql;
using GastroApi.Controllers;
using GastroApi.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//add authentication with keycloak
var keycloakConfig = builder.Configuration.GetSection("Keycloak");

//cors policy
builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(hostName => true);
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = keycloakConfig["Authority"];
    options.RequireHttpsMetadata = true; 
    options.Audience = keycloakConfig["Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        //ValidAudience = keycloakConfig["ClientId"],
        ValidIssuer = keycloakConfig["Authority"]
    };
});

//Add also DbContext to handle jsonb files
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<DbGastro>(options =>
//     options.UseNpgsql(connectionString));
builder.Services.AddTransient<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
}
);
// Add SQLKata QueryFactory
builder.Services.AddSingleton<QueryFactory>( provider =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("DefaultConnection");
    var connection = new NpgsqlConnection(connectionString); // Or SqlConnection for SQL Server
    var compiler = new PostgresCompiler(); // Or new SqlServerCompiler() for SQL Server
    var db = new QueryFactory(connection,compiler);
    db.Logger = compiled => { Console.WriteLine(compiled.ToString()); };
    return db;
});

// builder.Services.AddSingleton<ElasticsearchClient>(sp =>
//         {
//             var configuration = sp.GetRequiredService<IConfiguration>();
//             var elasticsearchUrl = configuration["Elasticsearch:Url"];
//             var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUrl))
//                 .DefaultIndex("gastroitems");

//             return new ElasticsearchClient(settings);
//         });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GastroApi", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{keycloakConfig["Authority"]}/protocol/openid-connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect" },
                    { "profile", "Profile information" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "openid", "profile" }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "testgastro v1");

        // Configure Swagger UI to use the OAuth2 token endpoint
        c.OAuthClientId(keycloakConfig["ClientId"]);
        c.OAuthClientSecret(keycloakConfig["ClientSecret"]);
        c.OAuthAppName("GastroApi - Swagger");
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
