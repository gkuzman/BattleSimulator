using BattleSimulator.Services.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BattleSimulator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BattleController(IMediator mediator)
        {
            _mediator = mediator;
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
            var request = new AddArmyRequest();
            var result = await _mediator.Send(request);
            return await Task.FromResult(Ok());
        }

        [HttpPut]
        public async Task<ActionResult> Reset()
        {
            return await Task.FromResult(Ok());
        }
    }
}
