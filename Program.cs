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
    db.Database.Migrate();
    
    var adminExists = db.Users.Any(u => u.Email == "admin@mail.com");
    User? admin = null;
    if (!adminExists)
    {
        admin = new User();
        admin.Email = "admin@mail.com";
        admin.PasswordHash = HashPassword("admin");
        admin.FirstName = "Admin";
        admin.LastName = "Admin";
        admin.Role = "Admin";
        db.Users.Add(admin);
        db.SaveChanges();
    }
    else
    {
        admin = db.Users.FirstOrDefault(u => u.Email == "admin@mail.com");
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

        var animal11 = new Animal();
        animal11.Name = "Charlie";
        animal11.Species = "Pies";
        animal11.Breed = "Jamnik";
        animal11.Age = 4;
        animal11.Gender = "Samiec";
        animal11.Description = "Charlie to wesoły i przyjazny jamnik. Ma charakterystyczny wygląd z długim ciałem i krótkimi nogami. Jest bardzo lojalny i przywiązany do rodziny. Uwielbia zabawę i spacery.";
        animal11.Status = "Available";
        animal11.AdmissionDate = DateTime.Now.AddDays(-35);
        animal11.PhotoUrl = "/images/animals/dachshund-1519374_1280.jpg";
        db.Animals.Add(animal11);

        var animal12 = new Animal();
        animal12.Name = "Nala";
        animal12.Species = "Kot";
        animal12.Breed = "Europejski";
        animal12.Age = 2;
        animal12.Gender = "Samica";
        animal12.Description = "Nala to elegancka i spokojna kotka. Ma piękne umaszczenie i przyjazne usposobienie. Idealna do mieszkania. Uwielbia pieszczoty i spokojne życie.";
        animal12.Status = "Available";
        animal12.AdmissionDate = DateTime.Now.AddDays(-18);
        animal12.PhotoUrl = "/images/animals/cat-2528935_1280.jpg";
        db.Animals.Add(animal12);

        var animal13 = new Animal();
        animal13.Name = "Rocky";
        animal13.Species = "Pies";
        animal13.Breed = "Mieszaniec";
        animal13.Age = 3;
        animal13.Gender = "Samiec";
        animal13.Description = "Rocky to energiczny i przyjazny pies. Jest bardzo towarzyski i dobrze dogaduje się z innymi zwierzętami. Wymaga regularnych spacerów i aktywności fizycznej.";
        animal13.Status = "Available";
        animal13.AdmissionDate = DateTime.Now.AddDays(-28);
        animal13.PhotoUrl = "/images/animals/dog-1839808_1280.jpg";
        db.Animals.Add(animal13);

        var animal14 = new Animal();
        animal14.Name = "Oscar";
        animal14.Species = "Kot";
        animal14.Breed = "Europejski";
        animal14.Age = 5;
        animal14.Gender = "Samiec";
        animal14.Description = "Oscar to dojrzały i spokojny kot. Ma łagodne usposobienie i jest idealny dla osób szukających spokojnego towarzysza. Uwielbia drzemki i pieszczoty.";
        animal14.Status = "Available";
        animal14.AdmissionDate = DateTime.Now.AddDays(-45);
        animal14.PhotoUrl = "/images/animals/cat-3113513_1280.jpg";
        db.Animals.Add(animal14);

        var animal15 = new Animal();
        animal15.Name = "Daisy";
        animal15.Species = "Pies";
        animal15.Breed = "Mieszaniec";
        animal15.Age = 2;
        animal15.Gender = "Samica";
        animal15.Description = "Daisy to radosna i przyjazna suczka. Jest bardzo inteligentna i szybko się uczy. Idealna dla rodzin z dziećmi. Uwielbia zabawę i aktywność.";
        animal15.Status = "Available";
        animal15.AdmissionDate = DateTime.Now.AddDays(-22);
        animal15.PhotoUrl = "/images/animals/dog-1854119_1280.jpg";
        db.Animals.Add(animal15);

        var animal16 = new Animal();
        animal16.Name = "Whiskers";
        animal16.Species = "Kot";
        animal16.Breed = "Europejski";
        animal16.Age = 1;
        animal16.Gender = "Samiec";
        animal16.Description = "Whiskers to młody i ciekawski kot. Jest bardzo aktywny i lubi się bawić. Szybko się przyzwyczaja do nowego otoczenia. Idealny dla osób szukających towarzysza do zabawy.";
        animal16.Status = "Available";
        animal16.AdmissionDate = DateTime.Now.AddDays(-12);
        animal16.PhotoUrl = "/images/animals/cat-6572630_1280.jpg";
        db.Animals.Add(animal16);

        var animal17 = new Animal();
        animal17.Name = "Buddy";
        animal17.Species = "Pies";
        animal17.Breed = "Mieszaniec";
        animal17.Age = 4;
        animal17.Gender = "Samiec";
        animal17.Description = "Buddy to lojalny i przyjazny pies. Jest bardzo towarzyski i dobrze dogaduje się z dziećmi. Wymaga regularnych spacerów i uwagi. Idealny dla aktywnych rodzin.";
        animal17.Status = "Available";
        animal17.AdmissionDate = DateTime.Now.AddDays(-32);
        animal17.PhotoUrl = "/images/animals/dog-3277414_1280.jpg";
        db.Animals.Add(animal17);

        var animal18 = new Animal();
        animal18.Name = "Lily";
        animal18.Species = "Kot";
        animal18.Breed = "Europejski";
        animal18.Age = 3;
        animal18.Gender = "Samica";
        animal18.Description = "Lily to spokojna i zrównoważona kotka. Ma piękne umaszczenie i przyjazne usposobienie. Idealna do mieszkania. Uwielbia pieszczoty i spokojne życie.";
        animal18.Status = "Available";
        animal18.AdmissionDate = DateTime.Now.AddDays(-26);
        animal18.PhotoUrl = "/images/animals/cat-7094808_1280.jpg";
        db.Animals.Add(animal18);

        var animal19 = new Animal();
        animal19.Name = "Zeus";
        animal19.Species = "Pies";
        animal19.Breed = "Mieszaniec";
        animal19.Age = 5;
        animal19.Gender = "Samiec";
        animal19.Description = "Zeus to dojrzały i spokojny pies. Jest bardzo lojalny i przywiązany do właściciela. Idealny dla osób szukających spokojnego towarzysza. Wymaga umiarkowanej aktywności.";
        animal19.Status = "Available";
        animal19.AdmissionDate = DateTime.Now.AddDays(-38);
        animal19.PhotoUrl = "/images/animals/dog-5213090_1280.jpg";
        db.Animals.Add(animal19);

        var animal20 = new Animal();
        animal20.Name = "Shadow";
        animal20.Species = "Kot";
        animal20.Breed = "Europejski";
        animal20.Age = 2;
        animal20.Gender = "Samiec";
        animal20.Description = "Shadow to elegancki i spokojny kot. Ma ciemne umaszczenie i przyjazne usposobienie. Idealny do mieszkania. Uwielbia pieszczoty i długie drzemki.";
        animal20.Status = "Available";
        animal20.AdmissionDate = DateTime.Now.AddDays(-19);
        animal20.PhotoUrl = "/images/animals/cat-8540772_1280.jpg";
        db.Animals.Add(animal20);

        var animal21 = new Animal();
        animal21.Name = "Coco";
        animal21.Species = "Pies";
        animal21.Breed = "Mieszaniec";
        animal21.Age = 1;
        animal21.Gender = "Samica";
        animal21.Description = "Coco to młoda i energiczna suczka. Jest bardzo inteligentna i szybko się uczy. Wymaga dużo ruchu i uwagi. Idealna dla aktywnych osób.";
        animal21.Status = "Available";
        animal21.AdmissionDate = DateTime.Now.AddDays(-8);
        animal21.PhotoUrl = "/images/animals/puppy-4608266_1280.jpg";
        db.Animals.Add(animal21);

        var animal22 = new Animal();
        animal22.Name = "Felix";
        animal22.Species = "Kot";
        animal22.Breed = "Europejski";
        animal22.Age = 4;
        animal22.Gender = "Samiec";
        animal22.Description = "Felix to dojrzały i spokojny kot. Ma łagodne usposobienie i jest idealny dla osób szukających spokojnego towarzysza. Uwielbia drzemki i pieszczoty.";
        animal22.Status = "Available";
        animal22.AdmissionDate = DateTime.Now.AddDays(-42);
        animal22.PhotoUrl = "/images/animals/cat-2528935_1280.jpg";
        db.Animals.Add(animal22);

        var animal23 = new Animal();
        animal23.Name = "Milo";
        animal23.Species = "Pies";
        animal23.Breed = "Mieszaniec";
        animal23.Age = 3;
        animal23.Gender = "Samiec";
        animal23.Description = "Milo to przyjazny i energiczny pies. Jest bardzo towarzyski i dobrze dogaduje się z dziećmi. Wymaga codziennej aktywności fizycznej. Idealny dla aktywnych rodzin.";
        animal23.Status = "Available";
        animal23.AdmissionDate = DateTime.Now.AddDays(-24);
        animal23.PhotoUrl = "/images/animals/dog-6389277_1280.jpg";
        db.Animals.Add(animal23);

        var animal24 = new Animal();
        animal24.Name = "Smok";
        animal24.Species = "Fretka";
        animal24.Breed = "Fretka domowa";
        animal24.Age = 1;
        animal24.Gender = "Samiec";
        animal24.Description = "Smok to młoda i ciekawska fretka. Jest bardzo aktywna i lubi się bawić. Wymaga dużo uwagi i stymulacji. Idealna dla doświadczonych właścicieli.";
        animal24.Status = "Available";
        animal24.AdmissionDate = DateTime.Now.AddDays(-14);
        animal24.PhotoUrl = "/images/animals/ferret-1215159_1280.jpg";
        db.Animals.Add(animal24);

        var animal25 = new Animal();
        animal25.Name = "Piggy";
        animal25.Species = "Świnka morska";
        animal25.Breed = "Świnka morska";
        animal25.Age = 1;
        animal25.Gender = "Samica";
        animal25.Description = "Piggy to przyjazna i spokojna świnka morska. Jest idealna dla dzieci. Wymaga odpowiedniej klatki i opieki. Uwielbia warzywa i owoce.";
        animal25.Status = "Available";
        animal25.AdmissionDate = DateTime.Now.AddDays(-16);
        animal25.PhotoUrl = "/images/animals/guinea-pig-2146091_1280.jpg";
        db.Animals.Add(animal25);

        var animal26 = new Animal();
        animal26.Name = "Rudy";
        animal26.Species = "Szczur";
        animal26.Breed = "Szczur domowy";
        animal26.Age = 1;
        animal26.Gender = "Samiec";
        animal26.Description = "Rudy to inteligentny i przyjazny szczur. Jest bardzo towarzyski i szybko się uczy. Wymaga odpowiedniej klatki i opieki. Idealny dla doświadczonych właścicieli.";
        animal26.Status = "Available";
        animal26.AdmissionDate = DateTime.Now.AddDays(-11);
        animal26.PhotoUrl = "/images/animals/rat-home-2899356_1280.jpg";
        db.Animals.Add(animal26);

        var animal27 = new Animal();
        animal27.Name = "Toby";
        animal27.Species = "Pies";
        animal27.Breed = "Mieszaniec";
        animal27.Age = 2;
        animal27.Gender = "Samiec";
        animal27.Description = "Toby to wesoły i przyjazny pies. Jest bardzo towarzyski i dobrze dogaduje się z innymi zwierzętami. Wymaga regularnych spacerów. Idealny dla aktywnych rodzin.";
        animal27.Status = "Available";
        animal27.AdmissionDate = DateTime.Now.AddDays(-21);
        animal27.PhotoUrl = "/images/animals/dog-6977210_1280.jpg";
        db.Animals.Add(animal27);

        var animal28 = new Animal();
        animal28.Name = "Ruby";
        animal28.Species = "Pies";
        animal28.Breed = "Mieszaniec";
        animal28.Age = 3;
        animal28.Gender = "Samica";
        animal28.Description = "Ruby to inteligentna i lojalna suczka. Jest bardzo przywiązana do właściciela. Wymaga regularnych ćwiczeń i uwagi. Idealna dla doświadczonych właścicieli.";
        animal28.Status = "Available";
        animal28.AdmissionDate = DateTime.Now.AddDays(-29);
        animal28.PhotoUrl = "/images/animals/dog-7630252_1280.jpg";
        db.Animals.Add(animal28);

        var animal29 = new Animal();
        animal29.Name = "Sam";
        animal29.Species = "Pies";
        animal29.Breed = "Mieszaniec";
        animal29.Age = 4;
        animal29.Gender = "Samiec";
        animal29.Description = "Sam to spokojny i dojrzały pies. Jest bardzo lojalny i przywiązany do rodziny. Wymaga umiarkowanej aktywności. Idealny dla osób szukających spokojnego towarzysza.";
        animal29.Status = "Available";
        animal29.AdmissionDate = DateTime.Now.AddDays(-36);
        animal29.PhotoUrl = "/images/animals/dog-8272860_1280.jpg";
        db.Animals.Add(animal29);

        db.SaveChanges();
    }

    if (!db.NewsPosts.Any() && admin != null)
    {
        var post1 = new NewsPost();
        post1.Title = "Wielka akcja adopcyjna - znajdź swojego przyjaciela!";
        post1.Content = "Zapraszamy wszystkich do udziału w naszej wielkiej akcji adopcyjnej! W schronisku czeka na Was wiele wspaniałych zwierząt, które szukają kochającego domu. Każde zwierzę zostało przebadane przez weterynarza i jest gotowe do adopcji.\n\nPodczas akcji oferujemy:\n- Bezpłatne konsultacje z naszymi specjalistami\n- Pomoc w wyborze odpowiedniego zwierzęcia\n- Wszystkie niezbędne dokumenty\n- Wsparcie po adopcji\n\nAkcja trwa do końca miesiąca. Nie czekaj - przyjdź i znajdź swojego nowego przyjaciela!";
        post1.Category = "Akcja";
        post1.PublicationDate = DateTime.Now.AddDays(-5);
        post1.AuthorId = admin.Id;
        db.NewsPosts.Add(post1);

        var post2 = new NewsPost();
        post2.Title = "Nowe zwierzęta w schronisku";
        post2.Content = "W ostatnim tygodniu do naszego schroniska trafiło kilka nowych zwierząt. Są wśród nich psy, koty oraz inne zwierzęta, które potrzebują nowego domu.\n\nWszystkie zwierzęta przeszły badania weterynaryjne i są zdrowe. Czekają na kochających właścicieli, którzy zapewnią im bezpieczny i ciepły dom.\n\nZachęcamy do odwiedzenia schroniska i poznania naszych podopiecznych. Każde zwierzę ma swoją historię i charakter, który warto poznać przed adopcją.";
        post2.Category = "Informacja";
        post2.PublicationDate = DateTime.Now.AddDays(-3);
        post2.AuthorId = admin.Id;
        db.NewsPosts.Add(post2);

        var post3 = new NewsPost();
        post3.Title = "Specjalna promocja - obniżone opłaty adopcyjne";
        post3.Content = "Mamy dla Was wspaniałą wiadomość! W tym miesiącu obniżyliśmy opłaty adopcyjne o 50% dla wszystkich zwierząt starszych niż 5 lat.\n\nStarsze zwierzęta często mają trudności ze znalezieniem domu, a są to wspaniałe, spokojne towarzysze, które idealnie nadają się dla osób szukających spokojnego przyjaciela.\n\nPromocja obowiązuje do końca miesiąca. Nie przegap tej okazji, aby dać starszemu zwierzęciu szansę na szczęśliwe życie w kochającym domu!";
        post3.Category = "Promocja";
        post3.PublicationDate = DateTime.Now.AddDays(-7);
        post3.AuthorId = admin.Id;
        db.NewsPosts.Add(post3);

        var post4 = new NewsPost();
        post4.Title = "Dzień otwarty w schronisku";
        post4.Content = "Serdecznie zapraszamy na dzień otwarty w naszym schronisku! To doskonała okazja, aby poznać naszych podopiecznych, zobaczyć jak funkcjonuje schronisko i dowiedzieć się więcej o procesie adopcji.\n\nPodczas dnia otwartego:\n- Zwiedzanie schroniska z przewodnikiem\n- Spotkania z naszymi zwierzętami\n- Prezentacje o opiece nad zwierzętami\n- Możliwość złożenia wniosku o adopcję\n- Kawiarenka z ciastem i kawą\n\nDzień otwarty odbędzie się w najbliższą sobotę w godzinach 10:00-16:00. Wstęp wolny!";
        post4.Category = "Akcja";
        post4.PublicationDate = DateTime.Now.AddDays(-10);
        post4.AuthorId = admin.Id;
        db.NewsPosts.Add(post4);

        var post5 = new NewsPost();
        post5.Title = "Sukces - 10 zwierząt znalazło dom w tym miesiącu!";
        post5.Content = "Mamy ogromną radość poinformować, że w tym miesiącu aż 10 naszych podopiecznych znalazło nowe, kochające domy! To wspaniała wiadomość, która pokazuje, że nasza praca przynosi efekty.\n\nWszystkim nowym właścicielom serdecznie gratulujemy i życzymy wiele radości ze swoich nowych przyjaciół. Dziękujemy również wszystkim, którzy wspierają nasze schronisko.\n\nNadal mamy wiele zwierząt czekających na adopcję. Jeśli myślisz o przygarnięciu zwierzaka, odwiedź nasze schronisko - na pewno znajdziesz idealnego towarzysza!";
        post5.Category = "Informacja";
        post5.PublicationDate = DateTime.Now.AddDays(-12);
        post5.AuthorId = admin.Id;
        db.NewsPosts.Add(post5);

        var post6 = new NewsPost();
        post6.Title = "Zbiórka karmy i akcesoriów dla zwierząt";
        post6.Content = "Organizujemy zbiórkę karmy, koców, zabawek i innych akcesoriów dla naszych podopiecznych. Każda pomoc jest dla nas cenna!\n\nPotrzebujemy:\n- Karma dla psów i kotów (sucha i mokra)\n- Koce i ręczniki\n- Zabawki dla zwierząt\n- Miski i smycze\n- Środki czystości\n\nMożna przynosić dary do schroniska w godzinach otwarcia lub skontaktować się z nami w sprawie odbioru większych darów. Dziękujemy za każde wsparcie!";
        post6.Category = "Akcja";
        post6.PublicationDate = DateTime.Now.AddDays(-15);
        post6.AuthorId = admin.Id;
        db.NewsPosts.Add(post6);

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
