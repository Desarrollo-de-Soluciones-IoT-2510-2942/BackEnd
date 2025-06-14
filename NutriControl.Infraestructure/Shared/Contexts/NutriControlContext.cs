
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NutriControl.Domain.Fields.Models.Entities;


namespace NutriControl.Contexts;


public class NutriControlContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public NutriControlContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NutriControlContext(DbContextOptions<NutriControlContext> options,IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Field> Fields { get; set; }
    
    public DbSet<Crop> Crops { get; set; }
    
    public DbSet<Recommendation> Recommendations { get; set; }
    
    public DbSet<History> Histories { get; set; }
    
    public DbSet<Device> Devices { get; set; }
    
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    
    public DbSet<Alert> Alerts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(_configuration["ConnectionStrings:NutriControlDB"],
                serverVersion);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        

        builder.Entity<User>().ToTable("User");
        builder.Entity<User>().HasKey(p => p.Id);
        builder.Entity<User>().Property(p => p.Username).IsRequired().HasMaxLength(50);
        builder.Entity<User>().Property(p => p.DniOrRuc).IsRequired();
        builder.Entity<User>().Property(p => p.EmailAddress).IsRequired();
        builder.Entity<User>().Property(p => p.Phone).IsRequired().HasMaxLength(12);
        builder.Entity<User>().Property(p => p.Role).IsRequired().HasMaxLength(20);
        builder.Entity<User>().Property(p => p.PasswordHashed).IsRequired();
        builder.Entity<User>().Property(p => p.ConfirmPassword).IsRequired();
        builder.Entity<User>().Property(p => p.CreateDate).IsRequired().HasDefaultValue(DateTime.Now);
        builder.Entity<User>().Property(p => p.IsActive).IsRequired().HasDefaultValue(true);

        
        builder.Entity<Subscription>().ToTable("Subscription");
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.UserId).IsRequired();
        builder.Entity<Subscription>().Property(s => s.PlanType).IsRequired();
        builder.Entity<Subscription>().Property(s => s.StartDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.EndDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<Field>().ToTable("Field");
        builder.Entity<Field>().HasKey(f => f.Id);
        builder.Entity<Field>().Property(f => f.UserId).IsRequired();
        builder.Entity<Field>().Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Field>().Property(f => f.Location).IsRequired().HasMaxLength(200);
        builder.Entity<Field>().Property(f => f.SoilType).IsRequired().HasMaxLength(50);
        builder.Entity<Field>().Property(f => f.Elevation).IsRequired();
        builder.Entity<Field>().Property(f => f.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<Crop>().ToTable("Crop");
        builder.Entity<Crop>().HasKey(c => c.Id);
        builder.Entity<Crop>().Property(c => c.FieldId).IsRequired();
        builder.Entity<Crop>().Property(c => c.Quantity).IsRequired();
        builder.Entity<Crop>().Property(c => c.Status).IsRequired().HasMaxLength(50);
        builder.Entity<Crop>().Property(f => f.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<Recommendation>().ToTable("Recommendation");
        builder.Entity<Recommendation>().HasKey(r => r.Id);
        builder.Entity<Recommendation>().Property(r => r.CropId).IsRequired();
        builder.Entity<Recommendation>().Property(r => r.Content).IsRequired().HasMaxLength(500);
        builder.Entity<Recommendation>().Property(r => r.Type).IsRequired().HasMaxLength(30);
        builder.Entity<Recommendation>().Property(r => r.Priority).IsRequired();
        builder.Entity<Recommendation>().Property(r => r.IsActive).IsRequired().HasDefaultValue(true);
        
        
        builder.Entity<History>().ToTable("History");
        builder.Entity<History>().HasKey(h => h.Id);
        builder.Entity<History>().Property(r => r.CropId).IsRequired();
        builder.Entity<History>().Property(h => h.StartDate).IsRequired();
        builder.Entity<History>().Property(h => h.EndDate).IsRequired();
        builder.Entity<History>().Property(h => h.SavingsType).IsRequired().HasMaxLength(50);
        builder.Entity<History>().Property(h => h.AmountSaved).IsRequired();
        builder.Entity<History>().Property(h => h.UnitOfMeasurement).IsRequired().HasMaxLength(20);
        builder.Entity<History>().Property(h => h.PercentageSaved).IsRequired().HasDefaultValue(0.0);
        builder.Entity<History>().Property(h => h.IsActive).IsRequired().HasDefaultValue(true);
        
        
        builder.Entity<Device>().ToTable("Device");
        builder.Entity<Device>().HasKey(d => d.Id);
        builder.Entity<Device>().Property(d => d.CropId).IsRequired();
        builder.Entity<Device>().Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Device>().Property(d => d.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<Sensor>().ToTable("Sensor");
        builder.Entity<Sensor>().HasKey(s => s.Id);
        builder.Entity<Sensor>().Property(s => s.DeviceId).IsRequired();
        builder.Entity<Sensor>().Property(s => s.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Sensor>().Property(s => s.UnitOfMeasurement).IsRequired().HasMaxLength(20);
        builder.Entity<Sensor>().Property(s => s.Status).IsRequired().HasMaxLength(20);
        builder.Entity<Sensor>().Property(s => s.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<SensorReading>().ToTable("SensorReading");
        builder.Entity<SensorReading>().HasKey(sr => sr.Id);
        builder.Entity<SensorReading>().Property(sr => sr.SensorId).IsRequired();
        builder.Entity<SensorReading>().Property(sr => sr.Timestamp).IsRequired();
        builder.Entity<SensorReading>().Property(sr => sr.Value).IsRequired();
        builder.Entity<SensorReading>().Property(sr => sr.IsActive).IsRequired().HasDefaultValue(true);
        
        builder.Entity<Alert>().ToTable("Alert");
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.DeviceId).IsRequired();
        builder.Entity<Alert>().Property(a => a.Timestamp).IsRequired();
        builder.Entity<Alert>().Property(a => a.Message).IsRequired().HasMaxLength(500);
        builder.Entity<Alert>().Property(a => a.Level).IsRequired().HasConversion<string>();
        builder.Entity<Alert>().Property(a => a.IsActive).IsRequired().HasDefaultValue(true);
        
    }
}