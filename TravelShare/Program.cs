using Microsoft.AspNetCore.HttpOverrides;
using TravelShare.Models.Expenses;
using TravelShare.Services;
using TravelShare.Services.Factories; // Add this using statement
using TravelShare.Services.FinanceMockData;
using TravelShare.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use the PORT environment variable (required for Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllersWithViews();

// ADD THIS - Required for SessionCurrentUserService
builder.Services.AddHttpContextAccessor();


builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // <-- ensures cookie only sent over HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // <-- same here for auth cookie
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Register application services
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
builder.Services.AddSingleton<IUserService, MockUserService>();
builder.Services.AddSingleton<MockUserService>();

// Add this line to register IUserFactory
builder.Services.AddScoped<IUserFactory, UserFactory>();

// Register MockExpensesData and expose it as IDataProvider
builder.Services.AddSingleton<MockExpensesData>();
builder.Services.AddSingleton<IDataProvider<Expense>, MockExpensesData>();
// Change these to Scoped (not Singleton) because they depend on SessionCurrentUserService which needs HttpContext
builder.Services.AddScoped<ICurrentUserService, SessionCurrentUserService>();
builder.Services.AddScoped<ExpensesService>();
builder.Services.AddScoped<IRead<Expense>>(sp => sp.GetRequiredService<ExpensesService>());
builder.Services.AddScoped<IWrite<Expense>>(sp => sp.GetRequiredService<ExpensesService>());
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Remove or comment out HTTPS redirection for Render (Render handles SSL)
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    if (!context.Request.IsHttps)
    {
        context.Request.Scheme = "https";
    }
    // Ukloni potencijalno opasne header-e
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");

    // Dodaj sigurnosne header-e
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";

    await next();
});

app.Run();