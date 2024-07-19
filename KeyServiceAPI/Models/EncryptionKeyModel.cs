namespace KeyServiceAPI.Models
{
    public class EncryptionKeyModel
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }   
    }
}
