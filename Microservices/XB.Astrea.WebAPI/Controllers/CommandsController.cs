using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace XB.Astrea.WebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ILogger<CommandsController> _logger;
        private readonly BackgroundServicesManager _bsController;

        public CommandsController(ILogger<CommandsController> logger, BackgroundServicesManager bsController)
        {
            _logger = logger;
            _bsController = bsController;
        }

        [HttpPost]
        public async Task<ActionResult> StopWorkers(string backgroundServiceClassName)
        {
            try
            {
                _logger.LogInformation("Attempt to stop background services");
                if (string.IsNullOrWhiteSpace(backgroundServiceClassName) || backgroundServiceClassName == "*")
                    await _bsController.StopAll();
                else
                    await _bsController.StopByName(backgroundServiceClassName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while stoping background services");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> StartWorkers(string backgroundServiceClassName)
        {
            try
            {
                _logger.LogInformation("Attempt to start background services");
                if (string.IsNullOrWhiteSpace(backgroundServiceClassName) || backgroundServiceClassName == "*")
                    await _bsController.StartAll();
                else
                    await _bsController.StartByName(backgroundServiceClassName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while start background services");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
