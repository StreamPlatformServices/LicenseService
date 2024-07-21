using KeyServiceAPI;
using KeyServiceAPI.Models;
using LicenseService.DataMappers;
using LicenseService.Models;
using LicenseService.Persistance.Data;
using LicenseService.Persistance.Repositories;
using LicenseService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LicenseService.Controllers
{
    //TODO: Add delete endpoint
    //TODO: Maybe don't update all fields. Think which should be updated.
    [ApiController]
    [Route("license")]
    public class LicenseController : ControllerBase
    {
        //TODO: Now Communication with APIGateway!!!!!!!!!!!!!!!!!!
        private readonly ILogger<LicenseController> _logger;
        private readonly ILicenseFasade _licenseService;
        //TODO: Create Module To comunicate with KeyServiceAPI
        //TODO: Create an interface to make boundry between Controllers KeyServiceAPI and Database (check in Clean Architecture)

        //TODO: Maybe only key service will be on Blockchain ? 
        public LicenseController(
            ILogger<LicenseController> logger,
            ILicenseFasade licenseService) //TODO: Create another class -> something like decorator...
        {
            _logger = logger;
            _licenseService = licenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenseByUserAndFileIdAsync([FromQuery] Guid userId, Guid fileId)
        {
            _logger.LogInformation("Start get license by user id procedure.");

            var result = await _licenseService.GetByUserAndFileIdAsync(userId, fileId);

            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound();
            }

            if (result.Data == null || result.Status == ResultStatus.Failed)
            {
                _logger.LogError("An unexpected error occurred while getting license.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation("Finished successfully get license by user id procedure.");
            return Ok(result.Data);
        }

        [HttpPut("{licenseId}")]
        public async Task<IActionResult> UpdateLicenseAsync(Guid licenseId, LicenseRequestModel licenseRequestModel)
        {
            _logger.LogInformation("Start update license procedure.");

            var result = await _licenseService.UpdateAsync(licenseId, licenseRequestModel.ToLicenseData());

            if (result == ResultStatus.NotFound)
            {
                return NotFound();
            }

            if (result == ResultStatus.Failed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation("Finished successfully update license procedure.");
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddLicenseAsync(LicenseRequestModel licenseRequestModel)
        {
            _logger.LogInformation($"Start add new license procedure for user: {licenseRequestModel.UserId}.");

            var result = await _licenseService.CreateAsync(licenseRequestModel.ToLicenseData());

            if (result == ResultStatus.Conflict)
            {
                return Conflict();
            }

            if (result == ResultStatus.Failed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation($"Finished successfully add new license procedure for user: {licenseRequestModel.UserId}."); //TODO: Log id in the rest of endpoints??
            return Ok();
        }

        [HttpDelete("{licenseId}")]
        public async Task<IActionResult> DeleteLicenseAsync(Guid licenseId)
        {
            _logger.LogInformation($"Start remove license procedure for user: {licenseId}.");

            var result = await _licenseService.DeleteAsync(licenseId);

            if (result == ResultStatus.NotFound)
            {
                return NotFound();
            }

            if (result == ResultStatus.Failed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            _logger.LogInformation($"Finished successfully add new license procedure for user: {licenseId}."); //TODO: Log id in the rest of endpoints??
            return Ok();
        }
    }
}
