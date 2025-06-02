using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.Models
{
    public class Application
    {
        [Key]
        public int id { get; set; }
        public string applicant_name { get; set; }
        public string email { get; set; }
        public string resume_url { get; set; }
        public int job_id { get; set; }
    }
}
