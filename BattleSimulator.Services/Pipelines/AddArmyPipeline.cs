using BattleSimulator.Entities.Enums;
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
            var result = default(AddArmyResponse);

            if (IsRequestValid(request, out string message))
            {
                _logger.LogInformation(message);
                result = await next();
                _logger.LogInformation("Army added");
            }
            else
            {
                result = new AddArmyResponse { OperationSuccessful = false };
                result.ErrorMessages.Add(message);
                _logger.LogError("Invalid request");
            }
            
            return result;
        }

        private bool IsRequestValid(AddArmyRequest request, out string message)
        {
            // TODO: read from options
            if (request.Units < 80 && request.Units > 100)
            {
                message = "Please provide number of units between 80 and 100";
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
