using LicenseService.Models;
using LicenseService.Persistance.Data;

namespace LicenseService.Services
{
    public interface ILicenseFasade
    {
        Task<(ResultStatus Status, IEnumerable<LicenseResponseModel> Data)> GetByUserIdAsync(Guid userId);
        Task<ResultStatus> CreateAsync(LicenseData licenseData);
        Task<ResultStatus> UpdateAsync(Guid uuid, LicenseData licenseData);
        Task<ResultStatus> DeleteAsync(Guid uuid);

    }
}
