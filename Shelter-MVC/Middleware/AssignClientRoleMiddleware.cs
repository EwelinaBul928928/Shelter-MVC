using Microsoft.AspNetCore.Identity;

namespace Shelter_MVC.Middleware
{
    public class AssignClientRoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AssignClientRoleMiddleware> _logger;

        public AssignClientRoleMiddleware(RequestDelegate next, ILogger<AssignClientRoleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    
                    if (!userRoles.Any())
                    {
                        if (!await roleManager.RoleExistsAsync("Client"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Client"));
                        }

                        var result = await userManager.AddToRoleAsync(user, "Client");
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("Przypisano rolę Client użytkownikowi {Email}", user.Email);
                            await signInManager.RefreshSignInAsync(user);
                        }
                        else
                        {
                            _logger.LogError("Nie udało się przypisać roli Client użytkownikowi {Email}", user.Email);
                        }
                    }
                }
            }

            await _next(context);
        }
    }

    public static class AssignClientRoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseAssignClientRole(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AssignClientRoleMiddleware>();
        }
    }
}
