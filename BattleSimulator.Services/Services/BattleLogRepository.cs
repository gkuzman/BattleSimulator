using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class BattleLogRepository : IBattleLogRepository
    {
        private readonly TrackingContext _trackingContext;
        private readonly ILogger<BattleLogRepository> _logger;

        public BattleLogRepository(TrackingContext trackingContext, ILogger<BattleLogRepository> logger)
        {
            _trackingContext = trackingContext;
            _logger = logger;
        }
        public async Task InsertBattleLogAsync(BattleLog battleLog, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                await _trackingContext.BattleLogs.AddAsync(battleLog);
                var result = await _trackingContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation($"Created new battle log with id: {battleLog.Id} battleId: {battleLog.BattleId} and job id: {battleLog.JobId}");
                }
                else
                {
                    _logger.LogError($"Failed to create new battle log with battleId: {battleLog.BattleId} and job id: {battleLog.JobId}");
                }
            }
        }
    }
}
