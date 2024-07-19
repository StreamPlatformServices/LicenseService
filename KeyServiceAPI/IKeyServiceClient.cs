using KeyServiceAPI.Models;

namespace KeyServiceAPI
{
    public interface IKeyServiceClient
    {
        Task<(ResultStatus Status, EncryptionKeyModel KeyData)> GetEncryptionKeyAsync(Guid fileId);
    }
}
