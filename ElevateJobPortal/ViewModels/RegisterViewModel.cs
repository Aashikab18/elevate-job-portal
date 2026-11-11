using System.ComponentModel.DataAnnotations;

namespace ElevateJobPortal.ViewModels;

public class RegisterViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = "";

    [Required]
    public string Role { get; set; } = "JobSeeker";

    [StringLength(150)]
    [Display(Name = "Company name")]
    public string? CompanyName { get; set; }
}