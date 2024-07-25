namespace LicenseService.Configuration
{
    public class KestrelSettings
    {
        public string ListeningIPv4Address { set; get; } = "localhost";
        public int PortNumber { set; get; } = 5010;
        public int TlsPortNumber { set; get; } = 5011;
        public bool UseTls { set; get; } = true;
    }
}
