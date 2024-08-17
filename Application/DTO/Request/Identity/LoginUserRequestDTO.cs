using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Identity;

using System.ComponentModel.DataAnnotations;

public class LoginUserRequestDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}

