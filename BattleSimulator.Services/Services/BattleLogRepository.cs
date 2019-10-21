using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class BattleLogRepository : IBattleLogRepository
    {
        private readonly TrackingContext _trackingContext;
        private readonly NonTrackingContext _nonTrackingContext;
        private readonly ILogger<BattleLogRepository> _logger;

        public BattleLogRepository(TrackingContext trackingContext, NonTrackingContext nonTrackingContext, ILogger<BattleLogRepository> logger)
        {
            _trackingContext = trackingContext;
            _nonTrackingContext = nonTrackingContext;
            _logger = logger;
        }
        public async Task InsertBattleLogAsync(IEnumerable<BattleLog> battleLogs)
        {
            await _trackingContext.BattleLogs.AddRangeAsync(battleLogs);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"BattleLogs saved in batch!");
            }
        }

        public async Task<BattleLog> GetLatestLogForBattle(int battleId)
        {
            var log = await _nonTrackingContext.BattleLogs.Where(x => x.BattleId == battleId).OrderByDescending(y => y.LogTime).FirstOrDefaultAsync();

            if (log is null)
            {
                _logger.LogError($"Could not find a battle log for battle with id {battleId}");
            }

            return log;
        }

        public async Task<List<string>> GetActionLogsForBattle(int battleId, string jobId)
        {
            var result = await _nonTrackingContext.BattleLogs.Where(x => x.BattleId == battleId && x.JobId == jobId).OrderBy(y => y.LogTime).Select(x => x.ActionTaken).ToListAsync();

            if (result is null)
            {
                _logger.LogError($"Could not find action logs for a battle with battle id {battleId} and job id {jobId}");
            }

            return result;
        }
    }
}

