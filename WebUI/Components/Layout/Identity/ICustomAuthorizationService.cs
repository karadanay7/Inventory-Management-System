using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Components.Layout.Identity;

public interface ICustomAuthorizationService
{
    bool CustomClaimChecker(ClaimsPrincipal user, string specificClaim);

}
