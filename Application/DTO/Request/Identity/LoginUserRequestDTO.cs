using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Identity;

public class LoginUserRequestDTO
{
    [EmailAddress]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character.")]
    [MinLength(8), MaxLength(100)] 
    public string Password { get; set; }


}
