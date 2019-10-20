using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class AttackReloadService : IAttackReloadService
    {
        private readonly IOptions<BattleOptions> _options;
        private readonly ILogger<AttackReloadService> _logger;

        public AttackReloadService(IOptions<BattleOptions> options, ILogger<AttackReloadService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task ReloadAsync(ArmyDTO attacker, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{attacker.Name} started reloading attack...");
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                attacker.ReloadTimeTotal = attacker.Units * _options.Value.ArmyReloadPerUnit;

                if (attacker.ElapsedReloadTime <= attacker.ReloadTimeTotal)
                {
                    await Task.Delay(_options.Value.ArmyReloadPerUnit);
                    attacker.ElapsedReloadTime = attacker.ElapsedReloadTime.Add(_options.Value.ArmyReloadPerUnit);
                }
                else
                {
                    _logger.LogInformation($"{attacker.Name} has reloaded...");
                    break;
                }
            }

            
            attacker.ElapsedReloadTime = TimeSpan.Zero;
            attacker.ReloadTimeTotal = TimeSpan.Zero;
        }
    }
}
