using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.DTOs
{
    public class UpdateJobDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
        public decimal Salary { get; set; }
    }
}
