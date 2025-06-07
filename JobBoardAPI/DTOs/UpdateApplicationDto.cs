using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.DTOs
{
    public class UpdateApplicationDto
    {
        [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
        public string? ApplicantName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ResumeFile { get; set; }

        public int? JobId { get; set; }
    }
}
