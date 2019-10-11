using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BattleSimulator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly ILogger<BattleController> _logger;

        public BattleController(ILogger<BattleController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        // TODO: strategy to enum
        public async Task<ActionResult> AddArmy(string name, int units, string strategy)
        {
            return await Task.FromResult(Ok());
        }

        // Seems that PUT is the most appropriate one, debatable with POST. Definitely not GET.
        [HttpPut]
        public async Task<ActionResult> Start()
        {
            return await Task.FromResult(Ok());
        }

        [HttpGet("{battleId}")]
        public async Task<ActionResult> GetLog(int battleId)
        {
            _logger.LogInformation("hello from logger");
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        public async Task<ActionResult> Reset()
        {
            return await Task.FromResult(Ok());
        }
    }
}
