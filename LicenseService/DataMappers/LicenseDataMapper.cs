using LicenseService.Persistance.Data;

namespace LicenseService.DataMappers
{
    public static class LicenseDataMapper
    {
        public static LicenseData ToLicenseData(this LicenseRequestModel model)
        {
            return new LicenseData
            {
                Uuid = Guid.NewGuid(),
                ContentId = model.ContentId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                MaxPlayCount = model.MaxPlayCount,
                MaxPlaybackDuration = model.MaxPlaybackDuration,
                UserId = model.UserId,
            };
        }
        public static IEnumerable<LicenseResponseModel> ToLicenseListResponseModel(this IEnumerable<LicenseData> data)
        {
            var licenses = new List<LicenseResponseModel>();
            foreach (var license in data)
            {
                licenses.Add(license.ToLicenseResponseModel());
            }
            return licenses;
        }
        public static LicenseResponseModel ToLicenseResponseModel(this LicenseData data)
        {
            return new LicenseResponseModel
            {
                Uuid = data.Uuid,
                ContentId = data.ContentId,
                StartTime = data.StartTime,
                EndTime = data.EndTime,
                MaxPlayCount = data.MaxPlayCount,
                MaxPlaybackDuration = data.MaxPlaybackDuration,
                UserId = data.UserId,
            };
        }
    }

}
