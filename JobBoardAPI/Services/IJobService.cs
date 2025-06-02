using JobBoardAPI.DTOs;
using JobBoardAPI.Helpers;

namespace JobBoardAPI.Services
{
    public interface IJobService
    {
        Task<ServiceResult> CreateJobAsync(CreateJobDto dto);
    }
}
