using System;
using System.Security.Claims;
using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Interface.Identity;
using Application.DTO.Response.Identity;
using Application.Extension.Identity;

namespace Infrastructure.Repository
{
    public class Account : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) return new ServiceResponse(false, "User already exists");

            var newUser = new ApplicationUser()
            {
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email,
                Name = model.Name
            };


            var result = CheckResult(await _userManager.CreateAsync(newUser, model.Password));
            if (!result.Flag) return result;

            else
                return await CreateUserClaims(model);
        }

        private async Task<ServiceResponse> CreateUserClaims(CreateUserRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Policy)) return new ServiceResponse(false, "No policy provided");
            Claim[] userClaims = [];
            if (model.Policy.Equals(Policy.AdminPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Name", model.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Delete", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManageUser", "true")
                ];

            }
            else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim("Name", model.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Delete", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManageUser", "false")
                ];

            }
            else if (model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Name", model.Name),
                    new Claim("Create", "false"),
                    new Claim("Update", "false"),
                    new Claim("Delete", "false"),
                    new Claim("Read", "true"),
                    new Claim("ManageUser", "false")
                ];

            }

            var result = CheckResult(await _userManager.AddClaimsAsync((await _userManager.FindByEmailAsync(model.Email)), userClaims));
            if (result.Flag) return new ServiceResponse(true, "User created successfully");
            else
                return result;
        }
        public async Task<ApplicationUser> FindUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

     public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
{
   

    var user = await _userManager.FindByEmailAsync(model.Email);
    if (user == null) return new ServiceResponse(false, "User not found");

    var verifyPassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
    if (!verifyPassword.Succeeded) return new ServiceResponse(false, "Incorrect Credentials");

    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
    if (!result.Succeeded) return new ServiceResponse(false, "Unknown error occurred");

    return new ServiceResponse(true, "Login successful");
}



        private async Task<ApplicationUser> FindUserById(string id) => await _userManager.FindByIdAsync(id);
        private static ServiceResponse CheckResult(IdentityResult result)
        {
            if (result.Succeeded) return new ServiceResponse(true, null);
            var errors = result.Errors.Select(e => e.Description);
            return new ServiceResponse(false, string.Join(Environment.NewLine, errors));
        }

        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
            var  UsersList = new List<GetUserWithClaimResponseDTO>();
            var allUsers = await _userManager.Users.ToListAsync();
            if (allUsers.Count == 0) return UsersList;
            foreach (var user in allUsers)
            {
               var currentUser = await _userManager.FindByIdAsync(user.Id);
                var getCurrentUserClaims = await _userManager.GetClaimsAsync(currentUser);
                if(getCurrentUserClaims.Any())
                UsersList.Add(new GetUserWithClaimResponseDTO()
                {
                    UserId = user.Id,
                    Email = getCurrentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    RoleName = getCurrentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    Name = getCurrentUserClaims.FirstOrDefault(c => c.Type == "Name")?.Value,
                    ManageUser = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "ManageUser")?.Value),
                    Create = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Create")?.Value),
                    Update = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Update")?.Value),
                    Delete = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Delete")?.Value),
                    Read = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Read")?.Value)

                });
                
            }

            return UsersList;
        }
         public async Task SetUpAsync()
        {
            try
            {
                var response = await CreateUserAsync(new CreateUserRequestDTO
                {
                    Name = "Administrator",
                    Email = "admin@admin.com",
                    Password = "Admin@123",
                    Policy = Policy.AdminPolicy
                });

                if (response.Flag)
                {
                    Console.WriteLine("Administrator user created successfully.");
                }
                else
                {
                    Console.WriteLine($"Error creating administrator user: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in SetUpAsync: {ex.Message}");
            }
        }



        public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return new ServiceResponse(false, "User not found");

            var oldClaims = await _userManager.GetClaimsAsync(user);
            var removeResult = await _userManager.RemoveClaimsAsync(user, oldClaims);
            if (!removeResult.Succeeded) return CheckResult(removeResult);

            var newClaims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, model.RoleName),
                new Claim("Name", model.Name),
                new Claim("Create", model.Create.ToString()),
                new Claim("Update", model.Update.ToString()),
                new Claim("Delete", model.Delete.ToString()),
                new Claim("Read", model.Read.ToString()),
                new Claim("ManageUser", model.ManageUser.ToString())
            };

            var addResult = await _userManager.AddClaimsAsync(user, newClaims);
            return CheckResult(addResult);
        }



       

    }
}