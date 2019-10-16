using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class BattleProcessor : IBattleProcessor
    {
        private readonly ILogger<BattleProcessor> _logger;

        public BattleProcessor(ILogger<BattleProcessor> logger)
        {
            _logger = logger;
        }
        public async Task<ArmyDTO> Attack(ArmyDTO attacker, List<ArmyDTO> armies)
        {
            await Task.Delay(1000);
            attacker.Units -= 5;
            _logger.LogInformation($"{attacker.Name} == {attacker.Units}");
            attacker.IsAttacking = false;
            return attacker;
        }
    }
}
