using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Components.Layout.Identity;

public class CustomAuthorizationService:ICustomAuthorizationService
{
    public bool CustomClaimChecker(ClaimsPrincipal user, string specificClaim)
    {
        if (!user.Identity!.IsAuthenticated) return false;
        var getClaim = user.HasClaim(x => x.Type == specificClaim);
        if (!getClaim) return false;
        var getState = user.Claims.FirstOrDefault(x => x.Type == specificClaim)!.Value;
        return Convert.ToBoolean(getState) is true;
        
    }

}
