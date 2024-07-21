namespace LicenseService.Models
{
    public class LicenseResponseModel
    {
        public Guid Uuid { get; set; }
        public Guid FileId { get; set; }
        public EncryptionKeyModel KeyData { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? MaxPlayCount { get; set; }
        public int? MaxPlaybackDuration { get; set; }
        public Guid UserId { get; set; }
    }
}
