using backend.Data;
using backend.Services;

var URL = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>();

// Add Redis caching
builder.Services.AddSingleton<RedisCacheService>();

// Add HttpClient
builder.Services.AddHttpClient();

// Add background service
builder.Services.AddHostedService<GitHubSyncService>();

// Add Repository Service
builder.Services.AddScoped<IRepositoryService, RepositoryService>();

// Configure CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(URL)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        // Check if database is empty
        if (!context.Repositories.Any())
        {
            // Trigger initial sync
            var repoService = services.GetRequiredService<IRepositoryService>();
            await repoService.SyncRepositoriesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

// Add error handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
    });
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();