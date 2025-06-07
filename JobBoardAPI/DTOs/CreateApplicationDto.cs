using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.DTOs
{
    public class CreateApplicationDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
        public string ApplicantName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Resume file is required.")]
        [DataType(DataType.Upload)]
        public IFormFile ResumeFile { get; set; }

        [Required(ErrorMessage = "Job ID is required.")]
        public int JobId { get; set; }
    }
}
