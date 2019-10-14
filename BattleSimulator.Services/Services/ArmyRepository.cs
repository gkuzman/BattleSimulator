using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Requests;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class ArmyRepository : IArmyRepository
    {
        private readonly IBattleRepository _battleRepository;
        private readonly TrackingContext _trackingContext;
        private readonly ILogger<ArmyRepository> _logger;

        public ArmyRepository(IBattleRepository battleRepository, TrackingContext trackingContext, ILogger<ArmyRepository> logger)
        {
            _battleRepository = battleRepository;
            _trackingContext = trackingContext;
            _logger = logger;
        }
        public async Task<bool> AddAnArmyAsync(Army request)
        {
            _logger.LogInformation($"Attempting to add an army with: {request.Name} to the database...");
            request.BattleId = await _battleRepository.GetInitializingBattleIdAsync();

            if (request.BattleId < 1)
            {
                request.BattleId = await _battleRepository.CreateBattleAsync();
            }

            await _trackingContext.AddAsync(request);
            var result = await _trackingContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation($"Adding an army with: {request.Name} successfull...");
                return true;
            }
            else
            {
                _logger.LogError($"Adding an army with: {request.Name} failed!");
                return false;
            }
        }
    }
}
