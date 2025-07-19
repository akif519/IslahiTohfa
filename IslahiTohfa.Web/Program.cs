using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using MudBlazor.Services;
using Blazored.LocalStorage;
using IslahiTohfa.Infrastructure.Data;
using IslahiTohfa.Infrastructure.Repositories;
using IslahiTohfa.Infrastructure.Services;
using IslahiTohfa.Application.Services;
using IslahiTohfa.Application.Mappings;
using IslahiTohfa.Domain.Interfaces;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using IslahiTohfa.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
await ConfigurePipeline(app);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Database Configuration
    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("IsladebhiTohfa.Infrastructure"));

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    // Authentication & Authorization
    ConfigureAuthentication(services, configuration);

    // AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // Repositories
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    services.AddScoped<IBookRepository, BookRepository>();
    services.AddScoped<IUserRepository, UserRepository>();

    // Application Services
    services.AddScoped<IBookService, BookService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IAnalyticsService, AnalyticsService>();

    // Infrastructure Services
    services.AddScoped<IFileStorageService, FileStorageService>();
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<ICacheService, CacheService>();

    // Validators
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    // Memory Cache
    services.AddMemoryCache();

    // HTTP Client
    services.AddHttpClient();

    // CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowedOrigins", policy =>
        {
            policy.WithOrigins(
                "https://localhost:5001",
                "https://localhost:7001",
                "http://localhost:5000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    // Controllers
    services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

    // API Documentation
    ConfigureSwagger(services);

    // Blazor Services
    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddMudServices();
    services.AddBlazoredLocalStorage();

    // Custom Authentication State Provider
    services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

    // Background Services
    services.AddHostedService<DatabaseMigrationService>();
    services.AddHostedService<AnalyticsBackgroundService>();

    // Configuration Options
    services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
    services.Configure<FileStorageSettings>(configuration.GetSection("FileStorage"));
    services.Configure<EmailSettings>(configuration.GetSection("Email"));

    // Health Checks
    services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>()
        .AddCheck<FileStorageHealthCheck>("file_storage")
        .AddCheck<EmailServiceHealthCheck>("email_service");

    // Rate Limiting
    services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("ApiRateLimit", config =>
        {
            config.PermitLimit = 100;
            config.Window = TimeSpan.FromMinutes(1);
            config.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            config.QueueLimit = 10;
        });
    });

    // Response Compression
    services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
        options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    });

    // Logging
    services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();

        if (!builder.Environment.IsDevelopment())
        {
            // Add production logging providers
            builder.AddApplicationInsights();
        }
    });
}

void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
{
    var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
    var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };

        // Configure JWT events for SignalR and Blazor Server
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin"));
        options.AddPolicy("ModeratorOrAdmin", policy =>
            policy.RequireRole("Moderator", "Admin"));
        options.AddPolicy("UserOrAbove", policy =>
            policy.RequireRole("User", "Moderator", "Admin"));
    });
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Islahi Tohfa API",
            Version = "v1",
            Description = "Islamic Educational Books Platform API",
            Contact = new OpenApiContact
            {
                Name = "Islahi Tohfa Team",
                Email = "support@islahitohfa.com",
                Url = new Uri("https://islahitohfa.com")
            }
        });

        // JWT Authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
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
                    }
                },
                Array.Empty<string>()
            }
        });

        // Include XML comments
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
}

async Task ConfigurePipeline(WebApplication app)
{
    // Exception Handling
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Islahi Tohfa API v1");
            c.RoutePrefix = "api/docs";
        });
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Security Headers
    app.UseSecurityHeaders();

    // HTTPS Redirection
    app.UseHttpsRedirection();

    // Static Files
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            // Cache static files for 1 year
            if (ctx.File.Name.EndsWith(".pdf") ||
                ctx.File.Name.EndsWith(".jpg") ||
                ctx.File.Name.EndsWith(".png") ||
                ctx.File.Name.EndsWith(".webp"))
            {
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
            }
        }
    });

    // Response Compression
    app.UseResponseCompression();

    // Rate Limiting
    app.UseRateLimiter();

    // Routing
    app.UseRouting();

    // CORS
    app.UseCors("AllowedOrigins");

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Custom Middleware
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<ErrorHandlingMiddleware>();

    // API Routes
    app.MapControllers().RequireRateLimiting("ApiRateLimit");

    // Health Checks
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(x => new
                {
                    name = x.Key,
                    status = x.Value.Status.ToString(),
                    exception = x.Value.Exception?.Message,
                    duration = x.Value.Duration.TotalMilliseconds
                })
            };
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    });

    // Blazor Routes
    app.MapRazorPages();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    // Database Migration and Seeding
    await EnsureDatabaseAsync(app);
}

async Task EnsureDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migration completed successfully");

        // Seed initial data if needed
        await SeedDataAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database");
        throw;
    }
}

async Task SeedDataAsync(ApplicationDbContext context, ILogger logger)
{
    try
    {
        // Check if data already exists
        if (await context.Categories.AnyAsync())
        {
            logger.LogInformation("Database already contains seed data");
            return;
        }

        // Seed categories
        var categories = new[]
        {
            new Category { Name = "Islamic Theology", NameArabic = "??????? ?????????", Description = "Books about Islamic faith and beliefs", IconClass = "fas fa-mosque", SortOrder = 1 },
            new Category { Name = "Quran & Tafsir", NameArabic = "?????? ????????", Description = "Quranic studies and interpretations", IconClass = "fas fa-book-open", SortOrder = 2 },
            new Category { Name = "Hadith", NameArabic = "?????? ??????", Description = "Prophetic traditions and their studies", IconClass = "fas fa-scroll", SortOrder = 3 },
            new Category { Name = "Islamic Jurisprudence", NameArabic = "????? ????????", Description = "Islamic law and jurisprudence", IconClass = "fas fa-balance-scale", SortOrder = 4 },
            new Category { Name = "Islamic History", NameArabic = "??????? ????????", Description = "History of Islam and Muslims", IconClass = "fas fa-landmark", SortOrder = 5 },
            new Category { Name = "Islamic Ethics", NameArabic = "??????? ?????????", Description = "Islamic morality and character building", IconClass = "fas fa-heart", SortOrder = 6 },
            new Category { Name = "Spirituality", NameArabic = "??????? ??????????", Description = "Islamic spirituality and purification", IconClass = "fas fa-hands-praying", SortOrder = 7 },
            new Category { Name = "Contemporary Issues", NameArabic = "??????? ????????", Description = "Modern Islamic perspectives", IconClass = "fas fa-globe", SortOrder = 8 }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        logger.LogInformation("Seed data added successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error seeding database");
        throw;
    }
}

// Configuration Classes
public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; } = 60;
    public int RefreshTokenExpiryInDays { get; set; } = 7;
}

public class FileStorageSettings
{
    public string BasePath { get; set; } = "wwwroot/uploads";
    public long MaxFileSize { get; set; } = 50 * 1024 * 1024; // 50MB
    public string[] AllowedExtensions { get; set; } = { ".pdf", ".jpg", ".png", ".webp" };
}

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; } = true;
}

// Extension Methods
public static class ServiceExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Add("Content-Security-Policy",
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com; " +
                "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
                "font-src 'self' https://fonts.gstatic.com; " +
                "img-src 'self' data: https:; " +
                "connect-src 'self'; " +
                "frame-ancestors 'none';");

            await next();
        });
    }
}

// Background Services
public class DatabaseMigrationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(IServiceProvider serviceProvider, ILogger<DatabaseMigrationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken); // Wait for application to start

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await context.Database.MigrateAsync(stoppingToken);
            _logger.LogInformation("Database migration completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database migration failed");
        }
    }
}

public class AnalyticsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AnalyticsBackgroundService> _logger;

    public AnalyticsBackgroundService(IServiceProvider serviceProvider, ILogger<AnalyticsBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var analyticsService = scope.ServiceProvider.GetRequiredService<IAnalyticsService>();

                // Process analytics data every hour
                await analyticsService.ProcessDailyAnalyticsAsync();
                _logger.LogInformation("Analytics processing completed");

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in analytics background service");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}