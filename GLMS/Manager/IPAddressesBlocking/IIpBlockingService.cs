using System.Net;

namespace GLMS.IPAddressesBlocking
{
    public interface IIpBlockingService
    {
        bool IsBlocked(IPAddress ipAddress);
    }
}
