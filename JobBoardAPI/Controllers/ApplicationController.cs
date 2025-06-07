using JobBoardAPI.DTOs;
using JobBoardAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JobBoardAPI.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IWebHostEnvironment _env;

        private const string UploadFolder = "uploads";
        private static readonly string[] PermittedExtensions = { ".pdf", ".doc", ".docx" };

        public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger, IWebHostEnvironment env)
        {
            _applicationService = applicationService;
            _logger = logger;
            _env = env;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] CreateApplicationDto dto)
        {
            _logger.LogInformation("Received create application request.");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            // Basic file null check
            if (dto.ResumeFile == null || dto.ResumeFile.Length == 0)
            {
                _logger.LogWarning("Resume file was not provided or is empty for applicant: {ApplicantName}", dto.ApplicantName);
                return BadRequest(new { message = "No resume file provided." });
            }

            // Validate extension
            var ext = Path.GetExtension(dto.ResumeFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !PermittedExtensions.Contains(ext))
            {
                _logger.LogWarning("Invalid file extension {Ext} for applicant: {ApplicantName}", ext, dto.ApplicantName);
                return BadRequest(new { message = "Invalid file type. Only .pdf, .doc, or .docx are allowed." });
            }

            try
            {
                // Save file to wwwroot/uploads
                var uploadsRoot = Path.Combine(_env.WebRootPath, UploadFolder);
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                // Sanitize filename
                var safeName = Regex.Replace(Path.GetFileNameWithoutExtension(dto.ResumeFile.FileName), @"[^a-zA-Z0-9_-]", "_");
                var uniqueFileName = $"{safeName}_{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadsRoot, uniqueFileName);

                // Copy the uploaded file to the target path
                await using var stream = new FileStream(filePath, FileMode.Create);
                await dto.ResumeFile.CopyToAsync(stream);

                var relativeUrl = $"/{UploadFolder}/{uniqueFileName}";

                _logger.LogInformation("Saved resume to {RelativeUrl} for applicant: {ApplicantName}", relativeUrl, dto.ApplicantName);

                // Call service, passing the relativeUrl
                var result = await _applicationService.CreateApplicationAsync(dto, relativeUrl);

                if (!result.Success)
                {
                    _logger.LogWarning("Failed to create application for {ApplicantName}. Reason: {Message}", dto.ApplicantName, result.Message);
                    return BadRequest(new { message = result.Message });
                }

                _logger.LogInformation("Application created successfully for applicant: {ApplicantName}", dto.ApplicantName);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating application with ApplicantName: {ApplicantName}", dto.ApplicantName);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Received request to retrieve all applications.");

            try
            {
                var result = await _applicationService.GetAllApplicationsAsync();
                if (!result.Success)
                {
                    _logger.LogError("Failed to retrieve applications. Reason: {Message}", result.Message);
                    return StatusCode(500, new { message = result.Message });
                }

                _logger.LogInformation("Retrieved {Count} applications.", result.Data?.Count ?? 0);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving applications.");
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Received request to get application with ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid application ID: {Id}", id);
                return BadRequest(new { message = "Invalid application ID." });
            }

            try
            {
                var result = await _applicationService.GetApplicationByIdAsync(id);
                if (!result.Success)
                {
                    _logger.LogWarning("Application not found with ID: {Id}", id);
                    return NotFound(new { message = result.Message });
                }

                _logger.LogInformation("Successfully retrieved application with ID: {Id}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving application with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateApplicationDto dto)
        {
            _logger.LogInformation("Received update application request for ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Validation failed", errors });
            }

            if (id <= 0)
            {
                _logger.LogWarning("Invalid application ID: {Id}", id);
                return BadRequest(new { message = "Invalid application ID." });
            }

            try
            {
                string? resumeRelativeUrl = null;

                // Basic file null check
                if (dto.ResumeFile != null && dto.ResumeFile.Length > 0)
                {
                    var ext = Path.GetExtension(dto.ResumeFile.FileName).ToLowerInvariant();
                    if (!PermittedExtensions.Contains(ext))
                    {
                        _logger.LogWarning("Invalid resume file extension: {Ext} for applicant: {ApplicantName}", ext, dto.ApplicantName);
                        return BadRequest(new { message = "Invalid file type. Only .pdf, .doc, or .docx are allowed." });
                    }

                    var uploadsRoot = Path.Combine(_env.WebRootPath, UploadFolder);
                    if (!Directory.Exists(uploadsRoot))
                        Directory.CreateDirectory(uploadsRoot);

                    var safeName = Regex.Replace(Path.GetFileNameWithoutExtension(dto.ResumeFile.FileName), @"[^a-zA-Z0-9_-]", "_");
                    var uniqueFileName = $"{safeName}_{Guid.NewGuid():N}{ext}";
                    var filePath = Path.Combine(uploadsRoot, uniqueFileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await dto.ResumeFile.CopyToAsync(stream);

                    resumeRelativeUrl = $"/{UploadFolder}/{uniqueFileName}";
                    _logger.LogInformation("Updated resume file stored at: {RelativeUrl} for applicant: {ApplicantName}", resumeRelativeUrl, dto.ApplicantName);
                }

                var result = await _applicationService.UpdateApplicationAsync(id, dto, resumeRelativeUrl);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to update application ID {Id}. Reason: {Message}", id, result.Message);
                    return BadRequest(new { message = result.Message });
                }

                _logger.LogInformation("Successfully updated application with ID: {Id}", id);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating application with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Received delete application request for ID: {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid application ID: {Id}", id);
                return BadRequest(new { message = "Invalid application ID." });
            }

            try
            {
                var result = await _applicationService.DeleteApplicationAsync(id);
                if (!result.Success)
                {
                    _logger.LogWarning("Failed to delete application with ID: {Id}. Reason: {Message}", id, result.Message);
                    return NotFound(new { message = result.Message });
                }

                _logger.LogInformation("Successfully deleted application with ID: {Id}", id);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting application with ID: {Id}", id);
                return StatusCode(500, new { message = "An internal server error occurred. Please try again later." });
            }
        }
    }
}
