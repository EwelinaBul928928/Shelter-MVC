using Microsoft.EntityFrameworkCore;
using Shelter.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ShelterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShelterDbContext>();
    db.Database.EnsureCreated();
    
    var adminExists = db.Users.Any(u => u.Email == "admin@mail.com");
    if (!adminExists)
    {
        var admin = new User();
        admin.Email = "admin@mail.com";
        admin.PasswordHash = HashPassword("admin");
        admin.FirstName = "Admin";
        admin.LastName = "Admin";
        admin.Role = "Admin";
        db.Users.Add(admin);
        db.SaveChanges();
    }
}

string HashPassword(string password)
{
    using (var sha256 = System.Security.Cryptography.SHA256.Create())
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return System.Convert.ToBase64String(hash);
    }
}

app.Run();
