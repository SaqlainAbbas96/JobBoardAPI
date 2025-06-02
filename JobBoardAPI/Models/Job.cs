using System.ComponentModel.DataAnnotations;

namespace JobBoardAPI.Models
{
    public class Job
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string company { get; set; }
        public string location { get; set; }
        public decimal salary { get; set; }
    }
}
