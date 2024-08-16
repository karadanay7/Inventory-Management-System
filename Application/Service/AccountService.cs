using System;
using System.Reflection.Metadata.Ecma335;
using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Application.DTO.Response.Identity;
using Application.Interface.Identity;

namespace Application.Service;

public class AccountService :IAccountService
{
   private readonly IAccount _account;

    public AccountService(IAccount account)
    {
        _account = account;
    }

    public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model) 
    => await _account.CreateUserAsync(model);
   

    public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
   => await _account.GetUsersWithClaimsAsync();

    public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
   => await _account.LoginAsync(model);

    public async Task SetUpAsync()
  => await _account.SetUpAsync();

    public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
   => await _account.UpdateUserAsync(model);

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
