using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Tasker.WebApp.Jobs
{
    public class CheckSystemHardwareJob : Job
    {
        private readonly ILogger<CheckSystemHardwareJob> _logger;
        public CheckSystemHardwareJob(IServiceProvider sp, TimeSpan runEvery, object payload)
            :base(runEvery, payload)
        {
            _logger = sp.GetService<ILogger<CheckSystemHardwareJob>>();
            this.Executing += CheckSystemHardwareJob_Executing;
            this.Executed += CheckSystemHardwareJob_Executed;
        }

        private void CheckSystemHardwareJob_Executed(object sender, JobExecutedEventArgs e)
        {
            
        }

        private void CheckSystemHardwareJob_Executing(object sender, JobExecutingEventArgs e)
        {
            e.Skip = true;
        }
        public override void ExecuteJob(object payload)
        {
            _logger.LogInformation($"Executing Job CheckSystemHardware Payload : {payload.ToString()} @ {DateTime.Now.ToString()}");
        }
    }
}
