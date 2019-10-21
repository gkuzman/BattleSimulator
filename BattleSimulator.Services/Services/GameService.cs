using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IArmyRepository _armyRepository;
        private readonly ILogger<GameService> _logger;
        private readonly IDbEntitiesToDtosMapper _mapper;
        private readonly IBattleProcessor _battleProcessor;
        private readonly IBattleLogRepository _battleLogRepository;
        private readonly List<ArmyDTO> _armies = new List<ArmyDTO>();
        private int _battleId = 0;
        private string _jobId = string.Empty;
        private BlockingCollection<Func<Task<(ArmyDTO, BattleLog)>>> _tasks;
        private int _test;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ConcurrentQueue<BattleLog> _battleLogs = new ConcurrentQueue<BattleLog>();
        private readonly List<BattleLog> _battleLogList = new List<BattleLog>();

        public GameService(IBattleRepository battleRepository,
            IArmyRepository armyRepository,
            ILogger<GameService> logger,
            IDbEntitiesToDtosMapper mapper,
            IBattleProcessor battleProcessor,
            IBattleLogRepository battleLogRepository)
        {
            _battleRepository = battleRepository;
            _armyRepository = armyRepository;
            _logger = logger;
            _mapper = mapper;
            _battleProcessor = battleProcessor;
            _battleLogRepository = battleLogRepository;
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

                using (_tasks = new BlockingCollection<Func<Task<(ArmyDTO, BattleLog)>>>(new ConcurrentQueue<Func<Task<(ArmyDTO, BattleLog)>>>(), _armies.Count))
                {
                    await StartTheBattle();

                    while (!_tasks.IsCompleted)
                    {
                        // do not proceed until all tasks are run
                    }
                }

                GetBattleLogs();
                await _battleLogRepository.InsertBattleLogAsync(_battleLogList);

                var sb = new StringBuilder();
                foreach (var army in _armies)
                {
                    sb.AppendLine($"{army.Name} finished the battle with {army.Units}");
                }

                _logger.LogInformation(sb.ToString());

                await _battleRepository.UpdateBattleAsync(_battleId, BattleStatus.Finished, _jobId);
            }
            else
            {
                _logger.LogInformation($"Unable to start a game with battle id: {_battleId} and hangfire job id {_jobId}");
            }

            _cts.Dispose();
        }

        private async Task StartTheBattle()
        {
            StartProducing();
            await StartConsuming();
        }

        private void StartProducing()
        {
            foreach (var army in _armies)
            {
                _tasks.Add(async () => await AttackAsync(army));
            }
        }

        private async Task StartConsuming()
        {
            foreach (var task in _tasks.GetConsumingEnumerable())
            {
                GetBattleLogs();
                await SaveLogsInBatch();
                _ = task.Invoke().ContinueWith(x =>
                {
                    if (!_tasks.IsAddingCompleted)
                        Enqueue(x.Result);
                }, _cts.Token);
            }
        }

        private async ValueTask SaveLogsInBatch()
        {
            if (_battleLogList.Count() > 10)
            {
                await _battleLogRepository.InsertBattleLogAsync(_battleLogList);
                _battleLogList.Clear();
            }
        }

        private void GetBattleLogs()
        {
            while (_battleLogs.TryDequeue(out var battleLog))
            {
                _battleLogList.Add(battleLog);
            }
        }

        private void Enqueue((ArmyDTO army, BattleLog battleLog) parameters)
        {
            if (parameters.battleLog != null)
            {
                _battleLogs.Enqueue(parameters.battleLog);
            }
            if (_armies.Count(x => x.Units > 0) < 2)
            {
                _logger.LogInformation("Battle finished");
                _tasks?.CompleteAdding();
                _cts?.Cancel();
            }

            if (parameters.army.Units < 1)
            {
                return;
            }

            if (!_tasks.IsAddingCompleted || !_cts.IsCancellationRequested)
            {
                _tasks?.TryAdd(async () => await AttackAsync(parameters.army));
            }
        }

        private async Task<(ArmyDTO, BattleLog)> AttackAsync(ArmyDTO army)
        {
            return await _battleProcessor.AttackAsync(army, _armies, _battleId, _jobId, _cts.Token);
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
