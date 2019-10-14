using BattleSimulator.Entities.Enums;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
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

        public StartGameService(IBattleRepository battleRepository, ILogger<StartGameService> logger, IOptions<BattleOptions> options)
        {
            _battleRepository = battleRepository;
            _logger = logger;
            _options = options;
        }
        public async Task<StartGameResponse> Handle(StartGameRequest request, CancellationToken cancellationToken)
        {
            var result = new StartGameResponse();
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

            //TODO start job
            var a = await _battleRepository.ChangeBattleStatus(battle.Id, BattleStatus.InBattle);

            return result;
        }
    }
}
