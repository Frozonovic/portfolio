using backend.Services;

try
{
    Console.WriteLine("Starting Program.cs...");

    var builder = WebApplication.CreateBuilder(args);

    Console.WriteLine("Line 7 has been reached...");

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            policy => policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod());
    });

    Console.WriteLine("Line 18 has been reached...");

    // Register services
    builder.Services.AddControllers();
    builder.Services.AddHttpClient();
    builder.Services.AddHostedService<GitHubUpdateService>();

    Console.WriteLine("Line 28 has been reached...");

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
    }

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

    Console.WriteLine("App is running...");

    app.Run();

    Console.WriteLine("Program.cs is fully executed...");
}
catch (Exception ex)
{
    Console.WriteLine("🚨 Fatal Error in Program.cs 🚨");

    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");

    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    throw;
}