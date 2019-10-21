using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        public async Task InsertBattleLogAsync(IEnumerable<BattleLog> battleLogs)
        {
            await _trackingContext.BattleLogs.AddRangeAsync(battleLogs);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"BattleLogs saved in batch!");
            }
        }
    }
}

