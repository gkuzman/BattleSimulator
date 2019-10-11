using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Pipelines
{
    public class AddArmyPipeline : IPipelineBehavior<AddArmyRequest, AddArmyResponse>
    {
        private readonly ILogger<AddArmyPipeline> _logger;

        public AddArmyPipeline(ILogger<AddArmyPipeline> logger)
        {
            _logger = logger;
        }
        public async Task<AddArmyResponse> Handle(AddArmyRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<AddArmyResponse> next)
        {
            _logger.LogInformation("Attempting to add an army");
            var result = await next();
            _logger.LogInformation("Army added");
            return result;
        }
    }
}
