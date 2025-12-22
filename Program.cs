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

    if (!db.Animals.Any())
    {
        var animal1 = new Animal();
        animal1.Name = "Burek";
        animal1.Species = "Pies";
        animal1.Breed = "Mieszaniec";
        animal1.Age = 3;
        animal1.Gender = "Samiec";
        animal1.Description = "Burek to przyjazny i energiczny pies. Uwielbia spacery i zabawę. Jest bardzo towarzyski i dobrze dogaduje się z dziećmi. Wymaga codziennej aktywności fizycznej.";
        animal1.Status = "Available";
        animal1.AdmissionDate = DateTime.Now.AddMonths(-2);
        animal1.PhotoUrl = "/images/animals/dog-1787835_1280.jpg";
        db.Animals.Add(animal1);

        var animal2 = new Animal();
        animal2.Name = "Mruczek";
        animal2.Species = "Kot";
        animal2.Breed = "Europejski";
        animal2.Age = 2;
        animal2.Gender = "Samiec";
        animal2.Description = "Mruczek to spokojny i łagodny kot. Idealny do mieszkania. Uwielbia pieszczoty i długie drzemki. Jest bardzo towarzyski i przyjazny.";
        animal2.Status = "Available";
        animal2.AdmissionDate = DateTime.Now.AddMonths(-1);
        animal2.PhotoUrl = "/images/animals/cat-111793_1280.jpg";
        db.Animals.Add(animal2);

        var animal3 = new Animal();
        animal3.Name = "Luna";
        animal3.Species = "Pies";
        animal3.Breed = "Labrador";
        animal3.Age = 1;
        animal3.Gender = "Samica";
        animal3.Description = "Luna to młoda, energiczna suczka. Jest bardzo inteligentna i szybko się uczy. Wymaga dużo ruchu i uwagi. Idealna dla aktywnych rodzin.";
        animal3.Status = "Available";
        animal3.AdmissionDate = DateTime.Now.AddDays(-15);
        animal3.PhotoUrl = "/images/animals/puppy-1207816_1280.jpg";
        db.Animals.Add(animal3);

        var animal4 = new Animal();
        animal4.Name = "Kicia";
        animal4.Species = "Kot";
        animal4.Breed = "Perski";
        animal4.Age = 4;
        animal4.Gender = "Samica";
        animal4.Description = "Kicia to spokojna i elegancka kotka. Ma piękne, długie futro wymagające regularnej pielęgnacji. Jest idealna dla osób szukających spokojnego towarzysza.";
        animal4.Status = "Available";
        animal4.AdmissionDate = DateTime.Now.AddDays(-30);
        animal4.PhotoUrl = "/images/animals/cat-339400_1280.jpg";
        db.Animals.Add(animal4);

        var animal5 = new Animal();
        animal5.Name = "Rex";
        animal5.Species = "Pies";
        animal5.Breed = "Owczarek niemiecki";
        animal5.Age = 5;
        animal5.Gender = "Samiec";
        animal5.Description = "Rex to inteligentny i lojalny pies. Jest bardzo posłuszny i dobrze wyszkolony. Idealny dla doświadczonych właścicieli. Wymaga regularnych ćwiczeń.";
        animal5.Status = "Available";
        animal5.AdmissionDate = DateTime.Now.AddMonths(-3);
        animal5.PhotoUrl = "/images/animals/dog-2561134_1280.jpg";
        db.Animals.Add(animal5);

        var animal6 = new Animal();
        animal6.Name = "Bella";
        animal6.Species = "Pies";
        animal6.Breed = "Corgi";
        animal6.Age = 2;
        animal6.Gender = "Samica";
        animal6.Description = "Bella to wesoła i przyjazna suczka. Ma krótkie nogi i długie ciało. Jest bardzo inteligentna i łatwa w szkoleniu. Idealna dla rodzin z dziećmi.";
        animal6.Status = "Available";
        animal6.AdmissionDate = DateTime.Now.AddDays(-20);
        animal6.PhotoUrl = "/images/animals/corgi-6673343_1280.jpg";
        db.Animals.Add(animal6);

        var animal7 = new Animal();
        animal7.Name = "Max";
        animal7.Species = "Pies";
        animal7.Breed = "Jack Russell";
        animal7.Age = 3;
        animal7.Gender = "Samiec";
        animal7.Description = "Max to żywiołowy i pełen energii pies. Uwielbia zabawę i aktywność. Jest bardzo przyjazny i towarzyski. Wymaga dużo ruchu i uwagi.";
        animal7.Status = "Available";
        animal7.AdmissionDate = DateTime.Now.AddDays(-25);
        animal7.PhotoUrl = "/images/animals/jack-russell-2029214_1280.jpg";
        db.Animals.Add(animal7);

        var animal8 = new Animal();
        animal8.Name = "Simba";
        animal8.Species = "Kot";
        animal8.Breed = "Europejski";
        animal8.Age = 1;
        animal8.Gender = "Samiec";
        animal8.Description = "Simba to młody, ciekawski kot. Jest bardzo aktywny i lubi się bawić. Szybko się przyzwyczaja do nowego otoczenia. Idealny dla osób szukających towarzysza do zabawy.";
        animal8.Status = "Available";
        animal8.AdmissionDate = DateTime.Now.AddDays(-10);
        animal8.PhotoUrl = "/images/animals/simba-8618301_1280.jpg";
        db.Animals.Add(animal8);

        var animal9 = new Animal();
        animal9.Name = "Lucky";
        animal9.Species = "Pies";
        animal9.Breed = "Australian Shepherd";
        animal9.Age = 4;
        animal9.Gender = "Samiec";
        animal9.Description = "Lucky to inteligentny i aktywny pies. Jest bardzo lojalny i przywiązany do właściciela. Wymaga dużo ćwiczeń i stymulacji umysłowej. Idealny dla aktywnych osób.";
        animal9.Status = "Available";
        animal9.AdmissionDate = DateTime.Now.AddMonths(-4);
        animal9.PhotoUrl = "/images/animals/australian-shepherd-5902417_1280.jpg";
        db.Animals.Add(animal9);

        var animal10 = new Animal();
        animal10.Name = "Mia";
        animal10.Species = "Kot";
        animal10.Breed = "Europejski";
        animal10.Age = 3;
        animal10.Gender = "Samica";
        animal10.Description = "Mia to spokojna i zrównoważona kotka. Uwielbia pieszczoty i spokojne życie. Jest idealna do mieszkania. Nie wymaga dużo uwagi, ale chętnie spędza czas z właścicielem.";
        animal10.Status = "Available";
        animal10.AdmissionDate = DateTime.Now.AddDays(-40);
        animal10.PhotoUrl = "/images/animals/cat-1192026_1280.jpg";
        db.Animals.Add(animal10);

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
