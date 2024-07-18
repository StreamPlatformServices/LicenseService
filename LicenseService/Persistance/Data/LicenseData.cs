using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LicenseService.Persistance.Data
{
    [Index(nameof(ContentId), nameof(UserId), IsUnique = true)]
    public class LicenseData
    {
        [Key]
        public Guid Uuid { get; set; }
        public Guid ContentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? MaxPlayCount { get; set; }
        public int? MaxPlaybackDuration { get; set; }
        public Guid UserId { get; set; }
    }
}
