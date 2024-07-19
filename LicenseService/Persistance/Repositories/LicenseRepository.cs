using LicenseService.Models;
using LicenseService.Persistance.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace LicenseService.Persistance.Repositories
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly ILogger<LicenseRepository> _logger;
        private readonly LicenseDatabaseContext _licenseDatabaseContext;

        private const int UNIQUE_CONSTRAINT_VIOLATION_ERROR_NUMBER = 2627;
        private const int PRIMARY_KEY_VIOLATION_ERROR_NUMBER = 2601;

        public LicenseRepository(
            ILogger<LicenseRepository> logger,
            LicenseDatabaseContext licenseDatabaseContext) 
        {
            _logger = logger;
            _licenseDatabaseContext = licenseDatabaseContext;
        }

        public async Task<(ResultStatus Status, IEnumerable<LicenseData> Data)> GetByUserIdAsync(Guid userId)
        {
            //Create Decorator, Adapter, Proxy or other module to fill the key data
            var licenseDataResult = await _licenseDatabaseContext.Licenses
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (licenseDataResult == null || !licenseDataResult.Any())
            {
                _logger.LogInformation($"License for user with id: {userId} not found!");
                return (ResultStatus.NotFound, new List<LicenseData>());
            }

            return (ResultStatus.Success, licenseDataResult);
        }

        public async Task<ResultStatus> CreateAsync(LicenseData licenseData) //TODO: License model??
        {
            try
            {
                await _licenseDatabaseContext.Licenses.AddAsync(licenseData);
                await _licenseDatabaseContext.SaveChangesAsync();
                return ResultStatus.Success;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException sqliteEx)
                {
                    if (sqliteEx.SqliteErrorCode == SQLitePCL.raw.SQLITE_CONSTRAINT)
                    {
                        _logger.LogError($"A conflict occurred while updating the database with new key: {sqliteEx.Message}");
                        return ResultStatus.Conflict;
                    }
                }
                if (ex.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Number == UNIQUE_CONSTRAINT_VIOLATION_ERROR_NUMBER || sqlEx.Number == PRIMARY_KEY_VIOLATION_ERROR_NUMBER)
                    {
                        _logger.LogError($"A conflict accourd while updating the database: {sqlEx.Message}");
                        return ResultStatus.Conflict;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Unexpected license data update error: {ex.Message}");
            }

            return ResultStatus.Failed;
        }

        public async Task<ResultStatus> UpdateAsync(Guid uuid, LicenseData updatedLicense)
        {
            //TODO: make it Info or Debug????
            _logger.LogInformation($"Start update license with id: {uuid}");
            var license = await _licenseDatabaseContext.Licenses
                .FirstOrDefaultAsync(e => e.Uuid == uuid);

            if (license == null)
            {
                _logger.LogError($"License with id: {uuid} not found!");
                return ResultStatus.NotFound;
            }

            license.StartTime = updatedLicense.StartTime;
            license.EndTime = updatedLicense.EndTime;
            license.MaxPlayCount = updatedLicense.MaxPlayCount;
            license.MaxPlaybackDuration = updatedLicense.MaxPlaybackDuration;

            try
            {
                await _licenseDatabaseContext.SaveChangesAsync();
                _logger.LogInformation($"Update license with id: {uuid}. Succeed");
                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error while updating the database. Error message: {ex.Message}");
                return ResultStatus.Failed;   
            }
        }

        public async Task<ResultStatus> DeleteAsync(Guid uuid)
        {
            _logger.LogInformation($"Start removing license from the database.");
            var content = await _licenseDatabaseContext.Licenses.FindAsync(uuid);
            if (content == null)
            {
                return ResultStatus.NotFound;
            }

            try
            {
                _licenseDatabaseContext.Licenses.Remove(content);
                await _licenseDatabaseContext.SaveChangesAsync(); //TODO: How many state entries should return when successfull?
                _logger.LogInformation($"license successfully removed from the database.");
                return ResultStatus.Success;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Unexpected error while removing license from the database. Error message: {ex.Message}");
                return ResultStatus.Failed;
            }
        }
    }
}
