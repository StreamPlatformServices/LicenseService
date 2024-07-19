namespace KeyServiceAPI.Models
{
    public class ContentEncryptionKeyModel
    {
        public Guid Uuid { get; set; }
        public Guid FileId { get; set; }

        public EncryptionKeyModel EncryptionKey { get; set; }
    }
}
