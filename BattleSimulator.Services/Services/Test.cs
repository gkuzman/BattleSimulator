using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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
            for (int i = 0; i < 1000; i++)
            {
                _logger.LogInformation($"From hf job{i}");
                Thread.Sleep(1000);
            }
            
        }
    }
}
