using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Identity;

public class LoginUserRequestDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]?)(?=.*\d).{6,100}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one lowercase letter and one numeric digit.")]
    public string Password { get; set; } = string.Empty;
}
