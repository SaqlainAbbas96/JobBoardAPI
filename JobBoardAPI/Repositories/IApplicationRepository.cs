using JobBoardAPI.DTOs;
using JobBoardAPI.Models;

namespace JobBoardAPI.Repositories
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IEnumerable<ApplicationViewModel>> GetAllApplicationsAsync();
        Task<ApplicationViewModel> GetApplicationByIdAsync(int id);
    }
}
