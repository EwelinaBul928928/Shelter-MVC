using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shelter_MVC.Models;

namespace Shelter_MVC.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roleNames = { "Administrator", "Client" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@mail";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(newAdmin, "admin");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Administrator");
                }
            }

            await SeedAnimals(context);
        }

        private static async Task SeedAnimals(ApplicationDbContext context)
        {
            var animalCount = await context.Animals.CountAsync();
            
            if (animalCount >= 25)
            {
                return;
            }

            var animals = new List<Animal>
            {
                new Animal { Name = "Bella", Age = 3, Description = "Urocza suczka rasy Australian Shepherd. Bardzo energiczna i przyjazna, uwielbia zabawę i długie spacery. Idealna dla aktywnych rodzin.", PhotoUrl = "/picture/australian-shepherd-5902417_1280.jpg", IsAdopted = false },
                new Animal { Name = "Max", Age = 2, Description = "Wesoły Corgi o przyjaznym usposobieniu. Uwielbia towarzystwo ludzi i innych zwierząt. Doskonały towarzysz dla całej rodziny.", PhotoUrl = "/picture/corgi-6673343_1280.jpg", IsAdopted = false },
                new Animal { Name = "Luna", Age = 4, Description = "Spokojna i zrównoważona suczka rasy Dachshund. Idealna dla osób szukających spokojnego towarzysza. Bardzo lojalna i oddana.", PhotoUrl = "/picture/dachshund-1519374_1280.jpg", IsAdopted = false },
                new Animal { Name = "Rocky", Age = 5, Description = "Aktywny Jack Russell Terrier pełen energii. Wymaga dużo ruchu i uwagi. Doskonały dla osób aktywnych fizycznie.", PhotoUrl = "/picture/jack-russell-2029214_1280.jpg", IsAdopted = false },
                new Animal { Name = "Charlie", Age = 1, Description = "Młody, pełen życia pies. Bardzo towarzyski i chętny do nauki. Idealny dla rodzin z dziećmi.", PhotoUrl = "/picture/dog-1787835_1280.jpg", IsAdopted = false },
                new Animal { Name = "Daisy", Age = 3, Description = "Przyjazna suczka o łagodnym charakterze. Uwielbia pieszczoty i spokojne spacery. Doskonała dla osób starszych.", PhotoUrl = "/picture/dog-1839808_1280.jpg", IsAdopted = false },
                new Animal { Name = "Buddy", Age = 2, Description = "Wesoły i energiczny pies. Bardzo inteligentny i szybko się uczy. Idealny towarzysz dla aktywnych osób.", PhotoUrl = "/picture/dog-1854119_1280.jpg", IsAdopted = false },
                new Animal { Name = "Milo", Age = 4, Description = "Spokojny i zrównoważony pies o przyjaznym usposobieniu. Uwielbia zabawę i długie spacery. Doskonały dla rodzin.", PhotoUrl = "/picture/dog-2561134_1280.jpg", IsAdopted = false },
                new Animal { Name = "Lola", Age = 3, Description = "Urocza suczka pełna energii. Bardzo towarzyska i przyjazna. Idealna dla osób szukających aktywnego towarzysza.", PhotoUrl = "/picture/dog-3277414_1280.jpg", IsAdopted = false },
                new Animal { Name = "Zeus", Age = 5, Description = "Duży, spokojny pies o łagodnym charakterze. Bardzo lojalny i oddany. Doskonały stróż i towarzysz.", PhotoUrl = "/picture/dog-5213090_1280.jpg", IsAdopted = false },
                new Animal { Name = "Ruby", Age = 2, Description = "Młoda, energiczna suczka. Uwielbia zabawę i aktywność fizyczną. Idealna dla osób aktywnych.", PhotoUrl = "/picture/dog-6389277_1280.jpg", IsAdopted = false },
                new Animal { Name = "Oscar", Age = 4, Description = "Przyjazny pies o spokojnym usposobieniu. Uwielbia pieszczoty i spokojne chwile. Doskonały towarzysz.", PhotoUrl = "/picture/dog-6977210_1280.jpg", IsAdopted = false },
                new Animal { Name = "Lucy", Age = 3, Description = "Urocza suczka pełna życia. Bardzo towarzyska i chętna do zabawy. Idealna dla rodzin z dziećmi.", PhotoUrl = "/picture/dog-7630252_1280.jpg", IsAdopted = false },
                new Animal { Name = "Cooper", Age = 2, Description = "Młody, energiczny pies. Bardzo inteligentny i szybko się uczy. Doskonały dla osób aktywnych.", PhotoUrl = "/picture/dog-8272860_1280.jpg", IsAdopted = false },
                new Animal { Name = "Bella", Age = 1, Description = "Mała, urocza suczka. Pełna energii i chęci do zabawy. Idealna dla rodzin z dziećmi.", PhotoUrl = "/picture/puppy-1207816_1280.jpg", IsAdopted = false },
                new Animal { Name = "Max", Age = 1, Description = "Młody, wesoły szczeniak. Bardzo towarzyski i chętny do nauki. Doskonały towarzysz dla całej rodziny.", PhotoUrl = "/picture/puppy-4608266_1280.jpg", IsAdopted = false },
                new Animal { Name = "Whiskers", Age = 2, Description = "Spokojny i zrównoważony kot. Uwielbia pieszczoty i spokojne chwile. Idealny dla osób szukających spokojnego towarzysza.", PhotoUrl = "/picture/cat-111793_1280.jpg", IsAdopted = false },
                new Animal { Name = "Mia", Age = 3, Description = "Urocza kotka o przyjaznym usposobieniu. Bardzo towarzyska i chętna do zabawy. Doskonała dla rodzin.", PhotoUrl = "/picture/cat-1192026_1280.jpg", IsAdopted = false },
                new Animal { Name = "Oliver", Age = 4, Description = "Spokojny i zrównoważony kot. Uwielbia spokojne chwile i pieszczoty. Idealny dla osób starszych.", PhotoUrl = "/picture/cat-2528935_1280.jpg", IsAdopted = false },
                new Animal { Name = "Lily", Age = 2, Description = "Młoda, energiczna kotka. Pełna życia i chęci do zabawy. Idealna dla aktywnych rodzin.", PhotoUrl = "/picture/cat-3113513_1280.jpg", IsAdopted = false },
                new Animal { Name = "Charlie", Age = 3, Description = "Przyjazny kot o łagodnym charakterze. Uwielbia towarzystwo ludzi. Doskonały towarzysz.", PhotoUrl = "/picture/cat-339400_1280.jpg", IsAdopted = false },
                new Animal { Name = "Sophie", Age = 4, Description = "Spokojna i zrównoważona kotka. Idealna dla osób szukających spokojnego towarzysza. Bardzo lojalna.", PhotoUrl = "/picture/cat-6572630_1280.jpg", IsAdopted = false },
                new Animal { Name = "Simba", Age = 2, Description = "Młody, energiczny kot. Pełen życia i chęci do zabawy. Idealny dla rodzin z dziećmi.", PhotoUrl = "/picture/simba-8618301_1280.jpg", IsAdopted = false },
                new Animal { Name = "Luna", Age = 3, Description = "Urocza kotka o przyjaznym usposobieniu. Bardzo towarzyska i chętna do pieszczot. Doskonała dla całej rodziny.", PhotoUrl = "/picture/cat-7094808_1280.jpg", IsAdopted = false },
                new Animal { Name = "Max", Age = 4, Description = "Spokojny i zrównoważony kot. Uwielbia spokojne chwile i pieszczoty. Idealny dla osób starszych.", PhotoUrl = "/picture/cat-8540772_1280.jpg", IsAdopted = false },
                new Animal { Name = "Nibbles", Age = 1, Description = "Mała, urocza fretka pełna energii. Bardzo towarzyska i chętna do zabawy. Wymaga dużo uwagi i aktywności.", PhotoUrl = "/picture/ferret-1215159_1280.jpg", IsAdopted = false },
                new Animal { Name = "Peanut", Age = 2, Description = "Przyjazna świnka morska o łagodnym charakterze. Idealna dla dzieci. Uwielbia pieszczoty i spokojne chwile.", PhotoUrl = "/picture/guinea-pig-2146091_1280.jpg", IsAdopted = false },
                new Animal { Name = "Remi", Age = 1, Description = "Mały, inteligentny szczurek. Bardzo towarzyski i chętny do zabawy. Idealny dla osób szukających małego towarzysza.", PhotoUrl = "/picture/rat-home-2899356_1280.jpg", IsAdopted = false }
            };

            context.Animals.AddRange(animals);
            await context.SaveChangesAsync();
        }
    }
}