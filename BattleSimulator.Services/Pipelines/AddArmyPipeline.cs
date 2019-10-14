using BattleSimulator.Entities.Enums;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using BattleSimulator.Services.Services;
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
            var result = new AddArmyResponse();

            if (IsRequestValid(request, result))
            {
                _logger.LogInformation($"Attempting to add an army with the name: {request.Name}, number of units: {request.Units} and attack strategy: {request.Strategy}.");
                result = await next();
            }
            else
            {
                _logger.LogError($"Invalid request: {result.ErrorMessages.GetErrorMessagesFormated()}");
            }
            
            return result;
        }

        private bool IsRequestValid(AddArmyRequest request, AddArmyResponse result)
        {
            var minUnits = _options.Value.MinUnits;
            var maxUnits = _options.Value.MaxUnits;
            if (request.Units < minUnits || request.Units > maxUnits)
            {
                result.ErrorMessages.Add($"Please provide number of units between {minUnits} and {maxUnits}");
            }
            if (request.Strategy == Strategy.None)
            {
                result.ErrorMessages.Add("Please provide correct attack strategy");
            }

            return result.ErrorMessages.Count == 0;
        }
    }
}
