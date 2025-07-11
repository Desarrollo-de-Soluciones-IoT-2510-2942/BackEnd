using System.Reflection;
using _1_API.Mapper;
using _2_Domain.IAM.CommandServices;
using Application;
using Application.IAM.CommandServices;
using Domain;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NutriControl.Contexts;
using NutriControl.Domain.IAM.Repositories;
using NutriControl.Domain.IAM.Services;
using NutriControl.Infraestructure.Crop.Persistence;
using NutriControl.Infraestructure.IAM.Persistence;
using NutriControl.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Configurar CORS para permitir acceso desde cualquier origen (para desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Servicios base
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🔧 Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "APIs for NutriControl",
        Description = "ASP.NET Core Web API for NutriControl Platform",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Support",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Inyección de dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IFieldCommandService, FieldCommandService>();
builder.Services.AddScoped<IFieldQueryService, FieldQueryService>();

builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<ICropCommandService, CropCommandService>();
builder.Services.AddScoped<ICropQueryService, CropQueryService>();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();

// AutoMapper
builder.Services.AddAutoMapper(
    typeof(RequestToModels),
    typeof(ModelsToRequest),
    typeof(ModelsToResponse));

// Conexión a base de datos
var connectionString = builder.Configuration.GetConnectionString("NutriControlDB");
builder.Services.AddDbContext<NutriControlContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// 🔐 Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

var app = builder.Build();

// 🧱 Middleware global
app.UseMiddleware<ErrorHandlerMiddleware>();

// Asegurar que la base de datos exista
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<NutriControlContext>())
{
    context.Database.EnsureCreated();
}

// 📘 Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// ✅ CORS — ⚠️ IMPORTANTE: este debe ir antes de Controllers, Auth, etc.
app.UseCors("AllowAllPolicy");

// HTTPS Redirection
app.UseHttpsRedirection();

// 🔐 Auth
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado (si necesitas validación adicional)
app.UseMiddleware<AuthenticationMiddleware>();

// Controllers
app.MapControllers();

// 🚀 Ejecutar app
app.Run();
