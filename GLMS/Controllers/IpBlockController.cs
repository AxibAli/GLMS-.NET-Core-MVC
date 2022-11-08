using GLMS.IPAddressesBlocking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpBlockController : ControllerBase
    {
        //these are the end points
        [HttpGet("unblocked")]
        public string Unblocked()
        {
            return "Unblocked access";
        }
        [ServiceFilter(typeof(IpBlockActionFilter))]
        [HttpGet("blocked")]
        public string Blocked()
        {
            return "Blocked access";
        }
    }
}
