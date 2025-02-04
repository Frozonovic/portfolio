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