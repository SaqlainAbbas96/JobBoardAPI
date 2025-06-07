namespace JobBoardAPI.DTOs
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string ResumeUrl { get; set; }
        public int JobId { get; set; }
    }
}
