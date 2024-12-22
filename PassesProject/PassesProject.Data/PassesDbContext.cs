using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PassesProject.Data.Models;
using PassesProject.Data.StaticData;

namespace PassesProject.Data;

public class PassesDbContext : DbContext
{
    public PassesDbContext() { }
    public PassesDbContext(DbContextOptions<PassesDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        if (!optionsBuilder.IsConfigured)
        {
            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, false)
                .AddJsonFile($"appsettings.{environmentName}.json", true, false).Build();

            string connectionString = config.GetConnectionString("PassesDB");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasData(UserStaticData.GetUserStaticData());
    }
}