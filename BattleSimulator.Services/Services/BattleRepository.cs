using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<bool> ChangeBattleStatus(int battleId, BattleStatus battleStatus)
        {
            var success = false;
            var battle = await _trackingContext.Battles.FindAsync(battleId);


            if (battle is null)
            {
                _logger.LogError($"No battle with id: {battleId} was found.");
                return success;
            }

            var oldStatus = battle.BattleStatus;
            battle.BattleStatus = battleStatus;
            _trackingContext.Battles.Update(battle);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"Changed battle status from: {oldStatus} to: {battleStatus}");
                success = true;
            }
            else
            {
                _logger.LogError($"Failed to update battle with new status: {battleStatus}");
            }

            return success;
        }
    }
}
