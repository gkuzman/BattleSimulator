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
        private readonly IDbEntitiesToDtosMapper _mapper;
        private List<ArmyDTO> _armies = new List<ArmyDTO>();
        private int _battleId = 0;
        private string _jobId = string.Empty;

        public GameService(IBattleRepository battleRepository, IArmyRepository armyRepository, ILogger<GameService> logger, IDbEntitiesToDtosMapper mapper)
        {
            _battleRepository = battleRepository;
            _armyRepository = armyRepository;
            _logger = logger;
            _mapper = mapper;
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
                _armies.AddRange(_mapper.MapBattleLogToArmiesDtoList(log));
                _logger.LogInformation($"Seems that the app crashed and the job resumed on restart. " +
                    $"Entities are loaded from the latest log with battle id {log.BattleId}, job id {log.JobId} and time {log.LogTime}");
            }
            else
            {
                var armies = await _armyRepository.GetArmiesAsync(_battleId);
                _armies.AddRange(_mapper.MapDbArmiesToArmiesDtoList(armies));
                _logger.LogInformation($"No battle log for battle with id: {_battleId} and job id: {_jobId} found. Starting battle from scratch!");
            }
        }
    }
}
