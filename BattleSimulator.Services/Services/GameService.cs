using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IArmyRepository _armyRepository;
        private readonly ILogger<GameService> _logger;
        private List<ArmyDTO> _armies = new List<ArmyDTO>();
        private int _battleId = 0;
        private string _jobId = string.Empty;

        public GameService(IBattleRepository battleRepository, IArmyRepository armyRepository, ILogger<GameService> logger)
        {
            _battleRepository = battleRepository;
            _armyRepository = armyRepository;
            _logger = logger;
        }
        public async Task StartGameAsync(PerformContext performContext, int battleId)
        {
            _battleId = battleId;
            _jobId = performContext.BackgroundJob.Id;
            var battleUpdated = await _battleRepository.UpdateBattleAsync(_battleId, BattleStatus.InBattle, _jobId);

            if (battleUpdated)
            {
                _logger.LogInformation($"Starting a game with battle id: {_battleId} and hangfire job id {_jobId}");
                await LoadEntitiesAsync();
            }
            else
            {
                _logger.LogInformation($"Unable to start a game with battle id: {_battleId} and hangfire job id {_jobId}");
            }
        }

        private async Task LoadEntitiesAsync()
        {
            var log = await _battleRepository.GetBattleLog(_battleId, _jobId);
            if (log != null)
            {
                // TODO map
            }
            else
            {
                var armies = await _armyRepository.GetArmiesAsync(_battleId);
            }
        }
    }
}
