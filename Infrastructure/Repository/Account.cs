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
            var user = await FindUserByEmail(model.Email);
            if (user != null) return new ServiceResponse(false, "User already exists");

            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded) return CheckResult(result);

            return await CreateUserClaims(newUser, model.Policy);
        }

        private async Task<ServiceResponse> CreateUserClaims(ApplicationUser user, string policy)
        {
            if (string.IsNullOrEmpty(policy)) return new ServiceResponse(false, "No policy provided");

            var userClaims = policy.Equals(Policy.AdminPolicy, StringComparison.OrdinalIgnoreCase)
                ? new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Name", user.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Delete", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManageUser", "true")
                }
                : new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Name", user.Name),
                    new Claim("Create", "false"),
                    new Claim("Update", "false"),
                    new Claim("Delete", "false"),
                    new Claim("Read", "false")
                };

            var result = await _userManager.AddClaimsAsync(user, userClaims);
            return CheckResult(result);
        }

        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
            var usersWithClaims = new List<GetUserWithClaimResponseDTO>();
            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any()) continue;

                usersWithClaims.Add(new GetUserWithClaimResponseDTO
                {
                    UserId = user.Id,
                    Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    RoleName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                    Name = claims.FirstOrDefault(x => x.Type == "Name")?.Value,
                    ManageUser = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "ManageUser")?.Value),
                    Create = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "Create")?.Value),
                    Update = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "Update")?.Value),
                    Delete = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "Delete")?.Value),
                    Read = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "Read")?.Value)
                });
            }

            return usersWithClaims;
        }

        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user == null) return new ServiceResponse(false, "User not found");

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded) return new ServiceResponse(false, "Invalid credentials");

            return new ServiceResponse(true, "Login successful");
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

        private async Task<ApplicationUser> FindUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

        private static ServiceResponse CheckResult(IdentityResult result)
        {
            if (result.Succeeded) return new ServiceResponse(true, null);
            var errors = result.Errors.Select(e => e.Description);
            return new ServiceResponse(false, string.Join(Environment.NewLine, errors));
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

    }
}
