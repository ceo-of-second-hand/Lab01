using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ChartApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:61668"); // Замініть на ваш реальний базовий URL
});

//access to db via sql server using configuration settings
builder.Services.AddDbContext<KyivBarGuideContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//adding identity management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6; 
    options.Password.RequiredUniqueChars = 1; 
})
    .AddEntityFrameworkStores<KyivBarGuideContext>()
    .AddDefaultTokenProviders();

//adding identity management
//doubtful about that
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Час життя Cookie (30 днів)
    options.SlidingExpiration = true; // Оновлювати час життя при кожному запиті
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); //identity management
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bars}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
