using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Tasker.WebApp.Jobs
{
    public partial class CheckSystemHealthJob : Job
    {
        private readonly ILogger<CheckSystemHealthJob> _logger;
        public CheckSystemHealthJob(IServiceProvider sp, TimeSpan runEvery, object payload) 
            : base(runEvery,payload)
        {
            _logger = sp.GetService<ILogger<CheckSystemHealthJob>>();

            this.Executing += CheckSystemHealthJob_Executing;
            this.Executed += CheckSystemHealthJob_Executed;
        }

        private void CheckSystemHealthJob_Executed(object sender, JobExecutedEventArgs e)
        {
            _logger.LogInformation($"CheckSystemHealthJob_Executed {e.ExecutionId}");
        }

        private void CheckSystemHealthJob_Executing(object sender, JobExecutingEventArgs e)
        {
            _logger.LogInformation($"CheckSystemHealthJob_Executing {e.ExecutionId}");
        }

        public override void ExecuteJob(object payload)
        {
            
            Random r = new Random(DateTime.Now.Millisecond);
            var randomvalue = r.Next();
            if (randomvalue % 2 == 0)
                _logger.LogError(new Exception("something bad happened"), "Simulating Exception in CheckSystemHealthJob", r.Next());
            else
                _logger.LogInformation("All Systems are healthy", r.Next());
        }



    }
}
