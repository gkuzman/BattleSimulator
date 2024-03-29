﻿using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class BattleRepository : IBattleRepository
    {
        private readonly NonTrackingContext _nonTrackingContext;
        private readonly TrackingContext _trackingContext;
        private readonly ILogger<BattleRepository> _logger;

        public BattleRepository(NonTrackingContext nonTrackingContext, TrackingContext trackingContext, ILogger<BattleRepository> logger)
        {
            _nonTrackingContext = nonTrackingContext;
            _trackingContext = trackingContext;
            _logger = logger;
        }

        public async Task<Battle> GetInitializingBattleAsync()
        {
            var battle = await _nonTrackingContext.Battles.Include(b => b.Armies).FirstOrDefaultAsync(x => x.BattleStatus == BattleStatus.Initializing);

            if (battle is null)
            {
                _logger.LogWarning($"Existing battle with status: {BattleStatus.Initializing} not found");
            }
            else
            {
                _logger.LogInformation($"Found battle with status: {BattleStatus.Initializing} and id: {battle.Id}");
            }

            return battle;
        }

        public async Task<Battle> GetInProgressBattleAsync()
        {
            var battle = await _nonTrackingContext.Battles.Include(b => b.Armies).FirstOrDefaultAsync(x => x.BattleStatus == BattleStatus.InBattle);

            if (battle is null)
            {
                _logger.LogWarning($"Existing battle with status: {BattleStatus.InBattle} not found");
            }
            else
            {
                _logger.LogInformation($"Found battle with status: {BattleStatus.InBattle} and id: {battle.Id}");
            }

            return battle;
        }

        public async Task<BattleLog> GetBattleLog(int battleId, string jobId)
        {
            var query = _nonTrackingContext.BattleLogs.AsQueryable();
            var latestLog = await query.OrderByDescending(x => x.ActionTaken)
                .FirstOrDefaultAsync(x => x.BattleId == battleId && x.JobId == jobId);

            if (latestLog is null)
            {
                _logger.LogWarning($"Existing battle log with battleId: {battleId} and jobId {jobId} not found");
            }
            else
            {
                _logger.LogInformation($"Found battle log with battleId: {battleId} and jobId {jobId}");
            }

            return latestLog;
        }

        public async Task<int> CreateBattleAsync()
        {
            var battle = new Battle
            {
                BattleStatus = BattleStatus.Initializing,
            };

            await _trackingContext.Battles.AddAsync(battle);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"Created new battle with status: {BattleStatus.Initializing} and id: {battle.Id}");
            }
            else
            {
                _logger.LogError($"Failed to create new battle with status: {BattleStatus.Initializing}");
            }

            return battle.Id;
        }

        public async Task<bool> UpdateBattleAsync(int battleId, BattleStatus battleStatus, string jobId = "")
        {
            var battle = await _trackingContext.Battles.FindAsync(battleId);

            if (battle is null)
            {
                _logger.LogError($"No battle with id: {battleId} was found.");
                return false;
            }

            battle.BattleStatus = battleStatus;

            if (!string.IsNullOrEmpty(jobId))
            {
                battle.JobId = jobId;
            }

            await _trackingContext.SaveChangesAsync();

            return true;
        }
    }
}
