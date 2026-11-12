using System.ComponentModel.DataAnnotations;

namespace ElevateJobPortal.ViewModels;

public class ApplicationFormViewModel
{
    public int JobId { get; set; }
    public string JobTitle { get; set; } = "";

    [Required, StringLength(2000, MinimumLength = 10)]
    [Display(Name = "Cover letter")]
    public string CoverLetter { get; set; } = "";
}