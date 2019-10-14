using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<AddArmyResponse>> AddArmy(AddArmyRequest request)
        {
            var result = await _mediator.Send(request);

            return ProcessResult(result);
        }

        // Seems that PUT is the most appropriate one, debatable with POST. Definitely not GET.
        [HttpPut]
        public async Task<ActionResult> Start()
        {
            var request = new StartGameRequest();
            var result = await _mediator.Send(request);

            return ProcessResult(result);
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

        private ActionResult ProcessResult<T>(T result) where T : ResponseBase
        {
            if (!result.HasErrors)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
