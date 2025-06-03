using JobBoardAPI.DTOs;
using JobBoardAPI.Helpers;
using JobBoardAPI.Models;
using JobBoardAPI.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoardAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IGenericRepository<Job> _jobRepository;
        private readonly ILogger<JobService> _logger;

        public JobService(IGenericRepository<Job> jobRepository, ILogger<JobService> logger)
        {
            _jobRepository = jobRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> CreateJobAsync(CreateJobDto dto)
        {
            _logger.LogInformation("Creating job: {Title}", dto.Title);

            try
            {
                // Map DTO to Model class
                var job = new Job
                {
                    title = dto.Title,
                    description = dto.Description,
                    company = dto.Company,
                    location = dto.Location,
                    salary = dto.Salary
                };

                var success = await _jobRepository.AddAsync(job);
                if (!success)
                {
                    _logger.LogError("Job creation failed for title: {Title}", dto.Title);
                    return ServiceResult.Fail("Job creation Failed.");
                }

                _logger.LogInformation("Job created successfully: {Title}", dto.Title);
                return ServiceResult.Ok("Job created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while creating job: {Title}", dto.Title);
                return ServiceResult.Fail($"An error occurred while creating the job: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<JobDto>>> GetAllJobsAsync()
        {
            _logger.LogInformation("Fetching all jobs.");

            try
            {
                var jobs = await _jobRepository.GetAllAsync();

                var jobDtos = jobs.Select(job => new JobDto
                {
                    Id = job.id,
                    Title = job.title,
                    Description = job.description,
                    Company = job.company,
                    Location = job.location,
                    Salary = job.salary
                }).ToList();

                _logger.LogInformation("Retrieved {Count} jobs.", jobDtos.Count);
                return ServiceResult<List<JobDto>>.Ok(jobDtos, "Jobs retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while fetching jobs.");
                return ServiceResult<List<JobDto>>.Fail($"Error fetching jobs: {ex.Message}");
            }
        }

        public async Task<ServiceResult<JobDto>> GetJobByIdAsync(int id)
        {
            _logger.LogInformation("Fetching job with ID: {Id}", id);

            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    _logger.LogWarning("Job not found with ID: {Id}", id);
                    return ServiceResult<JobDto>.Fail("Job not found for the provided id.");
                }

                var dto = new JobDto
                {
                    Id = job.id,
                    Title = job.title,
                    Description = job.description,
                    Company = job.company,
                    Location = job.location,
                    Salary = job.salary
                };

                _logger.LogInformation("Job retrieved with ID: {Id}", id);
                return ServiceResult<JobDto>.Ok(dto, "Job retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while retrieving job with ID: {Id}", id);
                return ServiceResult<JobDto>.Fail($"Error retrieving a job: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UpdateJobAsync(UpdateJobDto dto)
        {
            _logger.LogInformation("Updating job with ID: {Id}", dto.Id);

            try
            {
                var job = await _jobRepository.GetByIdAsync(dto.Id);
                if (job == null)
                {
                    _logger.LogWarning("Job not found with ID: {Id}", dto.Id);
                    return ServiceResult.Fail("Job not found for the provided id.");
                }

                // Manual mapping
                job.title = dto.Title;
                job.description = dto.Description;
                job.company = dto.Company;
                job.location = dto.Location;
                job.salary = dto.Salary;

                await _jobRepository.UpdateAsync(job);

                _logger.LogInformation("Job updated successfully with ID: {Id}", dto.Id);
                return ServiceResult.Ok("Job updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating job with ID: {Id}", dto.Id);
                return ServiceResult.Fail($"An error occurred while updating the job: {ex.Message}");
            }
        }

        public async Task<ServiceResult> DeleteJobAsync(int id)
        {
            _logger.LogInformation("Deleting job with ID: {Id}", id);

            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    _logger.LogWarning("Job not found with ID: {Id}", id);
                    return ServiceResult.Fail("Job not found for the provided id.");
                }

                await _jobRepository.DeleteAsync(job);

                _logger.LogInformation("Job deleted successfully with ID: {Id}", id);
                return ServiceResult.Ok("Job deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting job with ID: {Id}", id);
                return ServiceResult.Fail($"Error deleting job: {ex.Message}");
            }
        }
    }
}
