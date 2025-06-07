using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.DTOs
{
    public class UpdateJobDto
    {
        [StringLength(100, ErrorMessage = "Title must be under 100 characters.")]
        public string? Title { get; set; }

        [StringLength(4000, ErrorMessage = "Description must be under 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Company name must be under 100 characters.")]
        public string? Company { get; set; }

        [StringLength(100, ErrorMessage = "Location must be under 100 characters.")]
        public string? Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative number.")]
        public decimal? Salary { get; set; }
    }
}
