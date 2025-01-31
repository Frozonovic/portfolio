var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") // Replace with your frontend URL
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
}

// Optionally enable HTTPS redirection if you want to handle HTTPS requests
if (!app.Environment.isDevelopment()) {
    app.UseHttpsRedirection();
}

app.UseRouting();

// Map controllers for API routes
app.MapControllers(); // This is important for API route mapping

// Optionally configure a default route if you need it
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
