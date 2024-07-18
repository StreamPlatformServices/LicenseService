﻿using LicenseService.Models;

namespace LicenseService.Persistance.Data
{
    public class LicenseResponseModel
    {
        public Guid Uuid { get; set; }
        public Guid ContentId { get; set; }
        public ContentEncryptionKey Key { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? MaxPlayCount { get; set; }
        public int? MaxPlaybackDuration { get; set; }
        public Guid UserId { get; set; }
    }
}
