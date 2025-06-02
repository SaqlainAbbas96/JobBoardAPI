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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobDto dto) 
        {
            _logger.LogInformation("Create job request.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try 
            {
                _logger.LogInformation("Call create job service.");

                var result = await _jobService.CreateJobAsync(dto);
                if (!result.Success)
                {
                    _logger.LogError("Job creation failed.");
                    return StatusCode(500, new { Message = result.Message });
                }

                _logger.LogInformation("Job created successfully response.");
                return Ok(new { Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("An internal server error occurred.");
                return StatusCode(500, new { Message = "An internal server error occurred. Please try again later." });
            }
        }
    }
}
