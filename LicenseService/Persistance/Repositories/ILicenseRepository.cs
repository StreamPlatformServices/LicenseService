using LicenseService.Models;
using LicenseService.Persistance.Data;

namespace LicenseService.Persistance.Repositories
{
    public interface ILicenseRepository
    {
        Task<(ResultStatus Status, IEnumerable<LicenseData> Data)> GetByUserIdAsync(Guid userId);
        Task<ResultStatus> CreateAsync(LicenseData licenseData);
        Task<ResultStatus> UpdateAsync(Guid uuid, LicenseData licenseData);
        Task<ResultStatus> DeleteAsync(Guid uuid);
    }
}
