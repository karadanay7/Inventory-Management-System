using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Identity;

using System.ComponentModel.DataAnnotations;

public class LoginUserRequestDTO
{

    
    public string Email { get; set; } = string.Empty;

  
    public string Password { get; set; } = string.Empty;
}

