using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PassesProject.Data;
using PassesProject.Data.StaticData;
using PassesProject.Services;
using PassesProject.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfigurationRoot config = BuildConfiguration(environmentName);

builder.Configuration.AddJsonConfig(builder.Environment.EnvironmentName);
string? dbConnection = builder.Configuration.GetConnectionString("PassesDB");
builder.Services.AddDbContext<PassesDbContext>(opts => opts.UseNpgsql(dbConnection).EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PassesProject.API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PassesService>();
builder.Services.AddScoped<PassesStaticData>();
builder.Services.AddScoped<EncryptionHelper>();

byte[] secretKey =
    Encoding.ASCII.GetBytes(config["AppSettings:Secret"] ??
                            throw new InvalidOperationException("JWT Secret is not found"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(token =>
{
    token.RequireHttpsMetadata = false;
    token.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["AppSettings:JWTIssuer"],
        ValidAudience = config["AppSettings:JWTAudience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
    token.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine(ctx);
            return ctx.Response.WriteAsync("result");
        }
    };
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.AllowAnyOrigin();
    c.AllowAnyMethod();
    c.AllowAnyHeader();
    c.WithExposedHeaders("Content-Disposition");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static IConfigurationRoot BuildConfiguration(string environmentName)
{
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, false)
        .AddJsonFile($"appsettings.{environmentName}.json", true, false)
        .Build();
}