var FRONTEND_PUBLIC = Environment.GetEnvironmentVariable("FRONTEND_PUBLIC");
var FRONTEND_PRIVATE = Environment.GetEnvironmentVariable("FRONTEND_PRIVATE");

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
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

// Register HttpClient
builder.Services.AddHttpClient();

// Register services, such as controllers and views
builder.Services.AddControllers(); // Use this instead of AddControllersWithViews for API-only apps

var app = builder.Build();

// Use CORS
app.UseCors("AllowFrontend");

// Configure middleware
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

// Map controllers for API routes
app.MapControllers(); // This is important for API route mapping

// Optionally configure a default route if you need it
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔧 **Set the port dynamically for Railway**
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();