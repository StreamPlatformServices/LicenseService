using KeyServiceAPI;
using KeyServiceAPI.Models;
using LicenseService.DataMappers;
using LicenseService.Models;
using LicenseService.Persistance.Data;
using LicenseService.Persistance.Repositories;
using System.ComponentModel;

namespace LicenseService.Services
{
    //TODO: Create EntityComponent!!!!!!!!!!! or Just create an entityModel in separate folder
    //TODO: It is less code with exceptions ()think if there is a need to change this approach(maybe not becasue you have example of usage Task.WhenAll with results)

    public class LicenseFasade : ILicenseFasade
    {
        ILicenseRepository _licenseRepository;
        IKeyServiceClient _keyServiceClient;
        public LicenseFasade(
            ILicenseRepository licenseRepository,
            IKeyServiceClient keyServiceClient) 
        {
            _licenseRepository = licenseRepository;
            _keyServiceClient = keyServiceClient;
        }

        public async Task<(ResultStatus Status, LicenseResponseModel? Data)> GetByUserAndFileIdAsync(Guid userId, Guid fileId)
        {
            var licenseResult = await _licenseRepository.GetByUserAndFileIdAsync(userId, fileId);

            if (licenseResult.Status != ResultStatus.Success)
            {
                return (licenseResult.Status, null);
            }

            var licenseModel = licenseResult.Data.ToLicenseResponseModel();

            var result = await UpdateLicenseWithEncryptionKeyAsync(licenseModel);

            if (result != ResultStatus.Success)
            {
                return (result, null);
            }

            return (ResultStatus.Success, licenseModel);
        }
        //TODO: Maybe this method will be not needed (Get License By FileId and UserId!!!!!) NOW!!!!!!!!!!!!!!!
        public async Task<(ResultStatus Status, IEnumerable<LicenseResponseModel> Data)> GetByUserIdAsync(Guid userId) //TOOD: return null when failed?
        {
            var licenseResult = await _licenseRepository.GetByUserIdAsync(userId);

            if (licenseResult.Status != ResultStatus.Success)
            {
                return (licenseResult.Status, new List<LicenseResponseModel>());
            }

            var tasks = new List<Task<ResultStatus>>();

            var licenseModels = licenseResult.Data.ToLicenseListResponseModel();

            foreach (var license in licenseModels) 
            {
                tasks.Add(UpdateLicenseWithEncryptionKeyAsync(license));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                if (result != ResultStatus.Success)
                {
                    return (result, new List<LicenseResponseModel>());
                }
            }

            return (ResultStatus.Success, licenseModels);
        }

        private async Task<ResultStatus> UpdateLicenseWithEncryptionKeyAsync(LicenseResponseModel licenseModel)
        {
            var keyResult = await _keyServiceClient.GetEncryptionKeyAsync(licenseModel.FileId);

            if (keyResult.Status != ResultStatus.Success)
            {
                return keyResult.Status;
            }

            licenseModel.KeyData = new Models.EncryptionKeyModel { Key = keyResult.KeyData.Key, IV = keyResult.KeyData.IV };

            return ResultStatus.Success;
        }

        public async Task<ResultStatus> CreateAsync(LicenseData licenseData)
        {
            //TODO: Check if key exists??? 
            return await _licenseRepository.CreateAsync(licenseData);
        }
        public async Task<ResultStatus> DeleteAsync(Guid uuid)
        {
            return await _licenseRepository.DeleteAsync(uuid);
        }
        public async Task<ResultStatus> UpdateAsync(Guid uuid, LicenseData licenseData)
        {
            return await _licenseRepository.UpdateAsync(uuid, licenseData);
        }

        
    }
}
