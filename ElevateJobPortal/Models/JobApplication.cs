namespace ElevateJobPortal.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; }
    public string ApplicantId { get; set; } = "";
    public ApplicationUser? Applicant { get; set; }
    public string CoverLetter { get; set; } = "";
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}