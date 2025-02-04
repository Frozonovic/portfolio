// Environment variables for CORS configuration
string FRONTEND_PUBLIC = Environment.GetEnvironmentVariable("FRONTEND_PUBLIC") ?? "https://localhost:3000";
string FRONTEND_PRIVATE = Environment.GetEnvironmentVariable("FRONTEND_PRIVATE") ?? "http://localhost:3000";

var builder = WebApplication.CreateBuilder(args);

// Service configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins(
            "http://localhost:3000",
            FRONTEND_PRIVATE,
            FRONTEND_PUBLIC)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.MapControllers();

// Configure port for Railway deployment
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();