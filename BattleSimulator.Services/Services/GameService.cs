using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly ILogger<GameService> _logger;

        public GameService(IBattleRepository battleRepository, ILogger<GameService> logger)
        {
            _battleRepository = battleRepository;
            _logger = logger;
        }
        public async Task StartGameAsync(PerformContext performContext, int battleId)
        {
            var battleUpdated = await _battleRepository.UpdateBattleAsync(battleId, BattleStatus.InBattle, performContext.BackgroundJob.Id);

            if (battleUpdated)
            {
                _logger.LogInformation($"Starting a game with battle id: {battleId} and hangfire job id {performContext.BackgroundJob.Id}");
                // TODO start game
            }
            else
            {
                _logger.LogInformation($"Unable to start a game with battle id: {battleId} and hangfire job id {performContext.BackgroundJob.Id}");
            }
        }
    }
}
