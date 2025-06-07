using JobBoardAPI.DTOs;
using JobBoardAPI.Helpers;
using JobBoardAPI.Models;
using JobBoardAPI.Repositories;

namespace JobBoardAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(IApplicationRepository applicationRepository, ILogger<ApplicationService> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> CreateApplicationAsync(CreateApplicationDto dto, string resumeRelativeUrl)
        {
            _logger.LogInformation("Creating application record for {ApplicantName}", dto.ApplicantName);

            try
            {
                // Map DTO to Model class
                var application = new Application
                {
                    applicant_name = dto.ApplicantName,
                    email = dto.Email,
                    resume_url = resumeRelativeUrl,
                    job_id = dto.JobId
                };

                var success = await _applicationRepository.AddAsync(application);
                if (!success)
                {
                    _logger.LogError("Application creation failed in repository for {ApplicantName}", dto.ApplicantName);
                    return ServiceResult.Fail("Application creation Failed.");
                }

                _logger.LogInformation("Application record saved for {ApplicantName}", dto.ApplicantName);
                return ServiceResult.Ok("Application created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while saving application for {ApplicantName}", dto.ApplicantName);
                return ServiceResult.Fail($"An error occurred while creating the application: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<ApplicationViewModel>>> GetAllApplicationsAsync()
        {
            _logger.LogInformation("Fetching all applications.");

            try
            {
                var applications = await _applicationRepository.GetAllApplicationsAsync();

                var applicationDtos = applications.Select(application => new ApplicationViewModel
                {
                    ApplicationId = application.ApplicationId,
                    ApplicantName = application.ApplicantName,
                    Email = application.Email,
                    ResumeUrl = application.ResumeUrl,
                    JobId = application.JobId,
                    JobTitle = application.JobTitle
                }).ToList();

                _logger.LogInformation("Retrieved {Count} applications.", applicationDtos.Count);
                return ServiceResult<List<ApplicationViewModel>>.Ok(applicationDtos, "Applications retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while fetching applications.");
                return ServiceResult<List<ApplicationViewModel>>.Fail($"Error fetching applications: {ex.Message}");
            }
        }

        public async Task<ServiceResult<ApplicationViewModel>> GetApplicationByIdAsync(int id)
        {
            _logger.LogInformation("Fetching application with ID: {Id}", id);

            try
            {
                var application = await _applicationRepository.GetApplicationByIdAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("Application not found with ID: {Id}", id);
                    return ServiceResult<ApplicationViewModel>.Fail("Application not found for the provided id.");
                }

                var dto = new ApplicationViewModel
                {
                    ApplicationId = application.ApplicationId,
                    ApplicantName = application.ApplicantName,
                    Email = application.Email,
                    ResumeUrl = application.ResumeUrl,
                    JobId = application.JobId,
                    JobTitle = application.JobTitle
                };

                _logger.LogInformation("Application retrieved with ID: {Id}", id);
                return ServiceResult<ApplicationViewModel>.Ok(dto, "Application retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while retrieving application with ID: {Id}", id);
                return ServiceResult<ApplicationViewModel>.Fail($"Error retrieving a application: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateApplicationAsync(int id, UpdateApplicationDto dto, string? resumeUrl)
        {
            _logger.LogInformation("Updating application with ID: {Id}", id);

            try
            {
                var application = await _applicationRepository.GetByIdAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("Application not found with ID: {Id}", id);
                    return ServiceResult.Fail("Application not found for the provided id.");
                }

                // Map DTO to Model class
                if (dto.ApplicantName != null)
                    application.applicant_name = dto.ApplicantName;

                if (dto.Email != null)
                    application.email = dto.Email;

                if (dto.JobId != null)
                    application.job_id = (int)dto.JobId;

                if (!string.IsNullOrWhiteSpace(resumeUrl))
                    application.resume_url = resumeUrl;

                await _applicationRepository.UpdateAsync(application);

                _logger.LogInformation("Application updated successfully for ID: {Id}", id);
                return ServiceResult.Ok("Application updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating application with ID: {Id}", id);
                return ServiceResult.Fail($"An error occurred while updating the application: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteApplicationAsync(int id)
        {
            _logger.LogInformation("Deleting application with ID: {Id}", id);

            try
            {
                var application = await _applicationRepository.GetByIdAsync(id);
                if (application == null)
                {
                    _logger.LogWarning("Application not found with ID: {Id}", id);
                    return ServiceResult.Fail("Application not found for the provided id.");
                }

                await _applicationRepository.DeleteAsync(application);

                _logger.LogInformation("Application deleted successfully with ID: {Id}", id);
                return ServiceResult.Ok("Application deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting application with ID: {Id}", id);
                return ServiceResult.Fail($"Error deleting application: {ex.Message}");
            }
        }
    }
}
