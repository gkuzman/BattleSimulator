using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BattleSimulator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
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
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        public async Task<ActionResult> Reset()
        {
            return await Task.FromResult(Ok());
        }
    }
}
