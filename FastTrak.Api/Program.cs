using AspNetCoreRateLimit;
using FastTrak.Api.Data;
using FastTrak.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog early so startup errors are captured
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog (replaces default logging)
    builder.Host.UseSerilog((context, config) => config
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .Enrich.FromLogContext());

    // Database
    builder.Services.AddDbContext<FastTrakDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("FastTrakDb")));

    // CORS - allow MAUI app and browser access
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Rate limiting
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddInMemoryRateLimiting();

    // OpenAPI/Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "FastTrak API",
            Version = "v1",
            Description = "REST API for FastTrak nutrition tracking app. Provides restaurant menus and nutrition data."
        });
    });

    var app = builder.Build();

    // Seed database on startup (only if empty)
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<FastTrakDbContext>();
        await DbSeeder.SeedAsync(db);
    }

    // Global exception handler - returns clean JSON instead of stack traces
    app.UseExceptionHandler(error => error.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred.",
            status = 500
        });
    }));

    // Security headers
    app.Use(async (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "DENY";
        await next();
    });

    // Rate limiting (must be early in pipeline)
    app.UseIpRateLimiting();

    // CORS
    app.UseCors("AllowAll");

    // Swagger UI (development only)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "FastTrak API v1");
        });
    }

    app.UseHttpsRedirection();

    // Serilog request logging
    app.UseSerilogRequestLogging();

    // Map our endpoints
    app.MapRestaurantEndpoints();
    app.MapMenuItemEndpoints();

    // Health check endpoint
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithTags("Health")
        .ExcludeFromDescription();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
