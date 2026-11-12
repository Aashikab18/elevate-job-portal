using System.ComponentModel.DataAnnotations;
using ElevateJobPortal.Models;

namespace ElevateJobPortal.ViewModels;

public class JobFormViewModel : IValidatableObject
{
    [Required, StringLength(150)]
    public string Title { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    [Required, StringLength(100)]
    public string Location { get; set; } = "";

    [Display(Name = "Job type")]
    public JobType JobType { get; set; }

    [Display(Name = "Minimum salary")]
    public decimal? SalaryMin { get; set; }

    [Display(Name = "Maximum salary")]
    public decimal? SalaryMax { get; set; }

    public DateOnly Deadline { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Deadline < DateOnly.FromDateTime(DateTime.Today))
            yield return new ValidationResult("Deadline must be today or a future date.", new[] { nameof(Deadline) });

        if (SalaryMin.HasValue && SalaryMax.HasValue && SalaryMax < SalaryMin)
            yield return new ValidationResult("Maximum salary must not be lower than minimum salary.", new[] { nameof(SalaryMax) });
    }
}