using Microsoft.AspNetCore.Mvc;

namespace XB.Astrea.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("healthy");
        }
    }
}
