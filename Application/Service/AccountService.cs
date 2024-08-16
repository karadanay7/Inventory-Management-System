using System;
using System.Reflection.Metadata.Ecma335;
using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Application.DTO.Response.Identity;

namespace Application.Service;

public class AccountService(IAccountService account) :IAccountService
{
    public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model) 
    => await account.CreateUserAsync(model);
   

    public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
   => await account.GetUsersWithClaimsAsync();

    public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
   => await account.LoginAsync(model);

    public async Task SetUpAsync()
  => await account.SetUpAsync();

    public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
   => await account.UpdateUserAsync(model);

//    private async Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivitiesAsync()
//    => await account.GetActivitiesAsync();

//     private async Task SaveActivityAsync(ActivityTrackerRequestDTO model)
//     => await account.SaveActivityAsync(model);
//     public async Task<IEnumerable<IGrouping<DateTime, ActivityTrackerResponseDTO>>>
//     {
//         var data = await GetActivitiesAsync().GroupBy(x => x.Date).AsEnumerable();
//         return data;
//     }
}
