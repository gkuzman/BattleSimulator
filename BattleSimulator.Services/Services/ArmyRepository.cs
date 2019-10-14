using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class ArmyRepository : IArmyRepository
    {
        private readonly IBattleRepository _battleRepository;
        private readonly TrackingContext _trackingContext;
        private readonly NonTrackingContext _nonTrackingContext;
        private readonly ILogger<ArmyRepository> _logger;

        public ArmyRepository(IBattleRepository battleRepository, TrackingContext trackingContext, NonTrackingContext nonTrackingContext, ILogger<ArmyRepository> logger)
        {
            _battleRepository = battleRepository;
            _trackingContext = trackingContext;
            _nonTrackingContext = nonTrackingContext;
            _logger = logger;
        }
        public async Task<bool> AddAnArmyAsync(Army request)
        {
            _logger.LogInformation($"Attempting to add an army with name: {request.Name} to the database...");
            var battle = await _battleRepository.GetInitializingBattleAsync();
            request.BattleId = battle?.Id ?? 0;

            if (battle is null)
            {
                request.BattleId = await _battleRepository.CreateBattleAsync();
            }

            if (battle?.Armies?.Count(x => x.BattleId == request.BattleId && x.Name == request.Name) > 0)
            {
                throw new Exception($"An army with the name {request.Name} already exist for the current battle. Please choose another army name.");
            }

            await _trackingContext.AddAsync(request);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"Adding an army with name: {request.Name} successful...");
                return true;
            }
            else
            {
                _logger.LogError($"Adding an army with name: {request.Name} failed!");
                return false;
            }
        }

        public async Task<bool> IsArmyAddingBlocked()
        {
            return await _nonTrackingContext.Battles.AnyAsync(x => x.BattleStatus == BattleStatus.InBattle);
        }
    }
}
