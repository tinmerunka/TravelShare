using TravelShare.Models.Expenses;
using TravelShare.Services;
using TravelShare.Services.FinanceMockData;
using TravelShare.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register application services as singletons for demo (mock data)
builder.Services.AddSingleton<IAuthenticationService, MockAuthenticationService>();
builder.Services.AddSingleton<IUserService, MockUserService>();

<<<<<<< Updated upstream
builder.Services.AddSingleton<IDataProvider<Expense>, MockExpensesData>();
builder.Services.AddSingleton<IWrite<Expense>,ExpensesService >();
builder.Services.AddSingleton<IRead<Expense>, ExpensesService>();
=======
builder.Services.AddSingleton<IDataProvider<Expense>,MockExpensesData>();
builder.Services.AddSingleton<IRead<Expense>,ExpensesService>();
builder.Services.AddSingleton<IWrite<Expense>, ExpensesService>();
>>>>>>> Stashed changes
builder.Services.AddSingleton<MockUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
