using ConcertApi.Auth;
using ConcertApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//chouse between running in .NET and DOCKER
            var host =
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                    ? "mysql"
                    : "localhost";
            var port =
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                    ? "3306"
                    : Environment.GetEnvironmentVariable("CONCERT_DB_PORT");

var connectionString =
                    $"server={host};" +
                    $"port={port};" +
                    $"database={System.Environment.GetEnvironmentVariable("CONCERT_DB_NAME")};" +
                    $"user={System.Environment.GetEnvironmentVariable("CONCERT_DB_USER")};" +
                    $"password={System.Environment.GetEnvironmentVariable("CONCERT_DB_USER_PASSWORD")}";

builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)

                )
            );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ConcertApi",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddScoped<JwtService>();

var key = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<RevocationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();