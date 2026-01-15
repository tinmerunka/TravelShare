using TravelShare.Models.Expenses;
using TravelShare.Services;
using TravelShare.Services.FinanceMockData;
using TravelShare.Services.Interfaces;
using TravelShare.Services.Factories;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use the PORT environment variable (required for Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Required for SessionCurrentUserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // ADDED: Enhanced security for session cookies
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Register application services
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
builder.Services.AddSingleton<IUserService, MockUserService>();
builder.Services.AddSingleton<MockUserService>();
builder.Services.AddScoped<IUserFactory, UserFactory>();

// Register MockExpensesData and expose it as IDataProvider
builder.Services.AddSingleton<MockExpensesData>();
builder.Services.AddSingleton<IDataProvider<Expense>, MockExpensesData>();
builder.Services.AddScoped<ICurrentUserService, SessionCurrentUserService>();
builder.Services.AddScoped<ExpensesService>();
builder.Services.AddScoped<IRead<Expense>>(sp => sp.GetRequiredService<ExpensesService>());
builder.Services.AddScoped<IWrite<Expense>>(sp => sp.GetRequiredService<ExpensesService>());
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

var isDevelopment = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (!isDevelopment)
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

// Security headers middleware
app.Use(async (context, next) =>
{
    string csp;

    if (isDevelopment)
    {
        csp = "default-src 'self'; " +
              "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
              "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
              "img-src 'self' data: https://images.unsplash.com; " +
              "font-src 'self' data: https://cdn.jsdelivr.net https://fonts.gstatic.com; " +
              "connect-src 'self' ws://localhost:* wss://localhost:* http://localhost:*; " +
              "frame-ancestors 'none'; " +
              "form-action 'self'; " +
              "base-uri 'self'; " +
              "object-src 'none'";
    }
    else
    {
        csp = "default-src 'self'; " +
              "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
              "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
              "img-src 'self' data: https://images.unsplash.com; " +
              "font-src 'self' data: https://cdn.jsdelivr.net https://fonts.gstatic.com; " +
              "connect-src 'self'; " +
              "frame-ancestors 'none'; " +
              "form-action 'self'; " +
              "base-uri 'self'; " +
              "object-src 'none'; " +
              "upgrade-insecure-requests";
    }

    context.Response.Headers.Append("Content-Security-Policy", csp);
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Permissions-Policy",
        "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");

    await next();
});

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();