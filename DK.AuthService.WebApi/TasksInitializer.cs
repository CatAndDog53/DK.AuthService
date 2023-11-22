using DK.AuthService.Infrastructure;
using DK.AuthService.Model;
using DK.AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DK.AuthService.WebApi
{
    public static class TasksInitializer
    {
        public static WebApplication SeedDefaultRolesAndUsers(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                using var authenticationService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                try
                {
                    var roles = context.Roles.FirstOrDefault();
                    if (roles == null) 
                    {
                        context.Roles.AddRange(
                            new IdentityRole { Name = PredefinedUserRoles.USER, NormalizedName = PredefinedUserRoles.USER.ToUpper() },
                            new IdentityRole { Name = PredefinedUserRoles.ADMIN, NormalizedName = PredefinedUserRoles.ADMIN.ToUpper() }
                            );
                        context.SaveChanges();
                    }

                    var users = context.Users.FirstOrDefault();
                    if ( users == null )
                    {
                        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

                        ApplicationUser applicationUser = new ApplicationUser();
                        Guid guid = Guid.NewGuid();
                        applicationUser.Id = guid.ToString();
                        applicationUser.UserName = "Admin";
                        applicationUser.Email = "admin@mail.co";
                        applicationUser.NormalizedUserName = applicationUser.UserName.ToUpper();
                        applicationUser.NormalizedEmail = applicationUser.Email.ToLower();
                        applicationUser.FirstName = "fname";
                        applicationUser.LastName = "lname";

                        context.Users.Add(applicationUser);

                        var hashedPassword = passwordHasher.HashPassword(applicationUser, "password");
                        applicationUser.SecurityStamp = Guid.NewGuid().ToString();
                        applicationUser.PasswordHash = hashedPassword;

                        context.SaveChanges();

                        context.UserRoles.AddRange(
                            new IdentityUserRole<string> 
                            { 
                                UserId = applicationUser.Id, 
                                RoleId = context.Roles.FirstOrDefault(role => role.Name == PredefinedUserRoles.USER).Id 
                            },
                            new IdentityUserRole<string>()
                            {
                                UserId = applicationUser.Id,
                                RoleId = context.Roles.FirstOrDefault(role => role.Name == PredefinedUserRoles.ADMIN).Id
                            }
                        );

                        context.SaveChanges();
                    }
                }
                catch (Exception) 
                {
                    throw;
                }
                return app;
            }
        }
    }
}
