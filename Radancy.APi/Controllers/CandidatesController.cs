using Core.CandidateService;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Radancy.APi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ILogger<CandidatesController> _logger;
        private readonly ICandidateService _candidateService;

        public CandidatesController(ILogger<CandidatesController> logger, ICandidateService candidateService)
        {
            _logger = logger;
            _candidateService = candidateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates([FromQuery] string fullName)
        {
            // Validation to allow only letters (a-z, A-Z) and spaces, but no numbers or special characters
            if (string.IsNullOrWhiteSpace(fullName) || !Regex.IsMatch(fullName, @"^[a-zA-Z ]+$"))
            {
                _logger.LogInformation("Invalid full name format: {FullName}", fullName);
                return BadRequest("Full name can only contain letters (a-z, A-Z). No spaces, numbers, or special characters allowed.");
            }

            var candidates = await _candidateService.SearchCandidatesAsync(fullName);
            _logger.LogInformation("Found {Count} candidates matching full name: {FullName}", candidates.Count, fullName);
            return Ok(candidates);
        }
    }
}