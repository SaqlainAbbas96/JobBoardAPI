using JobBoardAPI.Data;
using JobBoardAPI.DTOs;
using JobBoardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardAPI.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        private readonly AppDbContext _dbContext;
        public ApplicationRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ApplicationViewModel>> GetAllApplicationsAsync() 
        {
            var query = from app in _dbContext.applications
                        join job in _dbContext.jobs on app.job_id equals job.id
                        select new ApplicationViewModel
                        {
                            ApplicationId = app.id,
                            ApplicantName = app.applicant_name,
                            Email = app.email,
                            ResumeUrl = app.resume_url,
                            JobId = job.id,
                            JobTitle = job.title
                        };

            return await query.ToListAsync();
        }

        public async Task<ApplicationViewModel> GetApplicationByIdAsync(int id)
        {
            var query = from app in _dbContext.applications
                        join job in _dbContext.jobs on app.job_id equals job.id
                        where app.id == id
                        select new ApplicationViewModel
                        {
                            ApplicationId = app.id,
                            ApplicantName = app.applicant_name,
                            Email = app.email,
                            ResumeUrl = app.resume_url,
                            JobId = job.id,
                            JobTitle = job.title
                        };

            return await query.FirstOrDefaultAsync();
        }
    }
}
