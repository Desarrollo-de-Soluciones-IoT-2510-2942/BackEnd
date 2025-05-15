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

// Ad CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "APis form mange NutriControl",
        Description = "An ASP.NET Core Web API for managing ToDo finance",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // Aquí reemplaza la configuración de seguridad anterior por la nueva:
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
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


// Dependency injection
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






// AutoMapper
builder.Services.AddAutoMapper(
    typeof(RequestToModels),
    typeof(ModelsToRequest),
    typeof(ModelsToResponse));

// Conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("NutriControlDB");
builder.Services.AddDbContext<NutriControlContext>(
    dbContextOptions =>
    {
        dbContextOptions.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString)
        );
    });

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<NutriControlContext>())
{
    context.Database.EnsureCreated();
}

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<AuthenticationMiddleware>();
app.UseCors("AllowAllPolicy");

app.Run();