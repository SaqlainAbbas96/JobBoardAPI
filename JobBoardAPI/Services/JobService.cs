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
            _logger.LogInformation("Create job service.");

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

                _logger.LogInformation("Call create job repository.");

                var success = await _jobRepository.AddAsync(job);
                if (!success)
                {
                    _logger.LogError("Job creation Failed.");
                    return ServiceResult.Fail("Job creation Failed.");
                }

                _logger.LogInformation("Job created successfully.");
                return ServiceResult.Ok("Job created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the job. " + ex.Message);
                return ServiceResult.Fail($"An error occurred while creating the job: {ex.Message}");
            }
        }
    }
}
