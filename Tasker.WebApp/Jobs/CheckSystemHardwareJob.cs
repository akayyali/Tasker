using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Tasker.WebApp.Jobs
{
    public class CheckSystemHardwareJob : Job
    {
        private readonly ILogger<CheckSystemHardwareJob> _logger;
        public CheckSystemHardwareJob(IServiceProvider sp)
        {
            _logger = sp.GetService<ILogger<CheckSystemHardwareJob>>();
        }
        public override void Execute(object payload)
        {
            _logger.LogInformation($"Executing Job CheckSystemHardware Payload : {payload.ToString()} @ {DateTime.Now.ToString()}");
        }
    }
}
