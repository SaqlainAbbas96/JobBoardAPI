using JobBoardAPI.DTOs;
using JobBoardAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardAPI.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobController> _logger;

        public JobController(IJobService jobService, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto)
        {
            _logger.LogInformation("Received create job request.");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            try
            {
                _logger.LogInformation("Creating job with title: {Title}", dto.Title);

                var result = await _jobService.CreateJobAsync(dto);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to create job. Reason: {Message}", result.Message);
                    return BadRequest(new { message = result.Message });
                }

                _logger.LogInformation("Job created successfully with title: {Title}", dto.Title);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating job with title: {Title}", dto.Title);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Received request to retrieve all jobs.");

            try
            {
                var result = await _jobService.GetAllJobsAsync();
                if (!result.Success)
                {
                    _logger.LogError("Failed to retrieve jobs. Reason: {Message}", result.Message);
                    return StatusCode(500, new { message = result.Message });
                }

                _logger.LogInformation("Retrieved {Count} jobs.", result.Data?.Count ?? 0);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving jobs.");
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Received request to get job with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid job ID: {Id}", id);
                return BadRequest(new { message = "Invalid job ID." });
            }

            try
            {
                var result = await _jobService.GetJobByIdAsync(id);
                if (!result.Success)
                {
                    _logger.LogWarning("Job not found with ID: {Id}", id);
                    return NotFound(new { message = result.Message });
                }

                _logger.LogInformation("Successfully retrieved job with ID: {Id}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving job with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobDto dto)
        {
            _logger.LogInformation("Received update job request for ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            if (id <= 0)
            {
                _logger.LogWarning("Invalid job ID: {Id}", id);
                return BadRequest(new { message = "Invalid job ID." });
            }

            try
            {
                var result = await _jobService.UpdateJobAsync(id, dto);
                if (!result.Success) 
                {
                    _logger.LogWarning("Failed to update job. Reason: {Message}", result.Message);
                    return NotFound(new { message = result.Message });
                }

                _logger.LogInformation("Successfully updated job with ID: {Id}", id);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating job with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Received delete job request for ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid job ID: {Id}", id);
                return BadRequest(new { message = "Invalid job ID." });
            }

            try
            {
                var result = await _jobService.DeleteJobAsync(id);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to delete job with ID: {Id}. Reason: {Message}", id, result.Message);
                    return NotFound(new { message = result.Message });
                }

                _logger.LogInformation("Successfully deleted job with ID: {Id}", id);
                return Ok(new { message = result.Message });                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting job with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }
    }
}
