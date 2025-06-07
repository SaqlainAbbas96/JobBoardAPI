using JobBoardAPI.DTOs;
using JobBoardAPI.Helpers;

namespace JobBoardAPI.Services
{
    public interface IApplicationService
    {
        Task<ServiceResult> CreateApplicationAsync(CreateApplicationDto dto, string resumeRelativeUrl);
        Task<ServiceResult<List<ApplicationViewModel>>> GetAllApplicationsAsync();
        Task<ServiceResult<ApplicationViewModel>> GetApplicationByIdAsync(int id);
        Task<ServiceResult> UpdateApplicationAsync(int id, UpdateApplicationDto dto, string? resumeUrl);
        Task<ServiceResult> DeleteApplicationAsync(int id);
    }
}
