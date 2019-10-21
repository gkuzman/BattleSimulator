using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class StartGameService : IRequestHandler<StartGameRequest, StartGameResponse>
    {
        private readonly IBattleRepository _battleRepository;
        private readonly ILogger<StartGameService> _logger;
        private readonly IOptions<BattleOptions> _options;
        private readonly IBackgroundJobClient _jobClient;

        public StartGameService(IBattleRepository battleRepository, ILogger<StartGameService> logger, IOptions<BattleOptions> options, IBackgroundJobClient jobClient)
        {
            _battleRepository = battleRepository;
            _logger = logger;
            _options = options;
            _jobClient = jobClient;
        }
        public async Task<StartGameResponse> Handle(StartGameRequest request, CancellationToken cancellationToken)
        {
            var result = new StartGameResponse();

            if (request.Reset)
            {
                await RestartAGame(result);
            }
            else
            {
                await StartAGame(result);
            }
            
            return result;
        }

        private async Task StartAGame(StartGameResponse result)
        {
            _logger.LogInformation("Attempting to start a game");

            var battle = await _battleRepository.GetInitializingBattleAsync();

            if (battle is null)
            {
                result.ErrorMessages.Add("There is no battle ready to start. Please add armies to initialize battle.");
            }
            else if (battle.Armies?.Count() < _options.Value.MinimumArmies)
            {
                result.ErrorMessages.Add($"Cannot start a game with less than {_options.Value.MinimumArmies} in battle. Please add more armies to the current battle.");
            }
            else
            {
                _jobClient.Schedule<IGameService>(x => x.StartGameAsync(null, battle.Id), TimeSpan.FromSeconds(1));

                result.BattleId = battle.Id;
            }
        }

        private async Task RestartAGame(StartGameResponse result)
        {
            _logger.LogInformation("Attempting to restart a game");

            var battle = await _battleRepository.GetInProgressBattleAsync();

            if (battle is null)
            {
                result.ErrorMessages.Add("There is no battle in progress.");
            }
            else
            {
                _jobClient.Delete(battle.JobId);
                await _battleRepository.UpdateBattleAsync(battle.Id, Entities.Enums.BattleStatus.Initializing);
                _jobClient.Schedule<IGameService>(x => x.StartGameAsync(null, battle.Id), TimeSpan.FromSeconds(1));

                result.BattleId = battle.Id;
            }
        }
    }
}
