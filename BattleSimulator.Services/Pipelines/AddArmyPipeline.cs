using BattleSimulator.Entities.Enums;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Pipelines
{
    public class AddArmyPipeline : IPipelineBehavior<AddArmyRequest, AddArmyResponse>
    {
        private readonly ILogger<AddArmyPipeline> _logger;
        private readonly IOptions<ArmyOptions> _options;

        public AddArmyPipeline(ILogger<AddArmyPipeline> logger, IOptions<ArmyOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        public async Task<AddArmyResponse> Handle(AddArmyRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<AddArmyResponse> next)
        {
            var result = default(AddArmyResponse);

            if (IsRequestValid(request, out string message))
            {
                _logger.LogInformation(message);
                result = await next();
                _logger.LogInformation("Army added");
            }
            else
            {
                result = new AddArmyResponse();
                result.ErrorMessages.Add(message);
                _logger.LogError("Invalid request");
            }
            
            return result;
        }

        private bool IsRequestValid(AddArmyRequest request, out string message)
        {
            var minUnits = _options.Value.MinUnits;
            var maxUnits = _options.Value.MaxUnits;
            if (request.Units < minUnits && request.Units > maxUnits)
            {
                message = $"Please provide number of units between {minUnits} and {maxUnits}";
                return false;
            }
            if (request.Strategy == Strategy.None)
            {
                message = "Please provide correct attack strategy";
                return false;
            }

            message = $"Attempting to add an army with the name: {request.Name}, number of units: {request.Units} and attack strategy: {request.Strategy}.";
            return true;
        }
    }
}
