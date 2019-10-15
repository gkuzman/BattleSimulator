using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Services
{
    public class Test : ITest
    {
        private readonly ILogger<Test> _logger;

        public Test(ILogger<Test> logger)
        {
            _logger = logger;
        }
        public void TestF()
        {
            _logger.LogInformation("From hf job");
        }
    }
}
