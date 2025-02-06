using backend.Services;

Console.WriteLine("Starting Program.cs...");

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

Console.WriteLine("Line 16 has been reached...");

// Register services
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<GitHubUpdateService>();

Console.WriteLine("Line 22 has been reached...");

var app = builder.Build();

Console.WriteLine("Line 27 has been reached...");

// Use CORS
app.UseCors("AllowFrontend");

Console.WriteLine("Line 32 has been reached...");

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

Console.WriteLine("Line 45 has been reached...");

app.UseRouting();

// Map controllers for API routes
app.MapControllers();

// Optionally configure a default route if you need it
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🔧 **Set the port dynamically for Railway**
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();

Console.WriteLine("Program.cs is fully executed...");
