using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Runner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<bool> Get()
        {
            return true;
        }
    }
}
