namespace JobBoardAPI.DTOs
{
    public class ApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string ResumeUrl { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
    }
}
