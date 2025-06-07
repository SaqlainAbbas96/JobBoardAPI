using JobBoardAPI.DTOs;
using JobBoardAPI.Helpers;

namespace JobBoardAPI.Services
{
    public interface IJobService
    {
        Task<ServiceResult> CreateJobAsync(CreateJobDto dto);
        Task<ServiceResult<List<JobDto>>> GetAllJobsAsync();
        Task<ServiceResult<JobDto>> GetJobByIdAsync(int id);
        Task<ServiceResult> UpdateJobAsync(int id, UpdateJobDto dto);
        Task<ServiceResult> DeleteJobAsync(int id);
    }
}
