using ExperimentalTask.Models.Domain;
using ExperimentalTask.Models.DTO;
using ExperimentalTask.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExperimentalTask.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenticationService(RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.signInManager = signInManager; 
            this.userManager = userManager;                    
        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) 
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }
            //match password
            if(!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded) 
            { 
              var userRoles = await userManager.GetRolesAsync(user);
              var authClaims = new List<Claim>
              { 
                  new Claim(ClaimTypes.Name,user.UserName) 
              };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged in successfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on login in";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByIdAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var reult = await userManager.CreateAsync(user, model.Password); 
            if (!reult.Succeeded) 
            {
                status.StatusCode = 0;
                status.Message = "User Creation failed";
                return status;
            }
            //Role Management
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if(await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }
    }
}