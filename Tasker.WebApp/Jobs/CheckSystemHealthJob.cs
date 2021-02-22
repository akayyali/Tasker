using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Tasker.WebApp.Jobs
{
    public class CheckSystemHealthJob : Job
    {
        private readonly ILogger<CheckSystemHealthJob> _logger;
        public CheckSystemHealthJob(IServiceProvider sp)
        {
            _logger = sp.GetService<ILogger<CheckSystemHealthJob>>();
        }

        public override void Execute(object payload)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            var randomvalue = r.Next();
            if(randomvalue % 2 == 0)
                _logger.LogError(new Exception("something bad happened"), "Simulating Exception in CheckSystemHealthJob", r.Next());
            else
                _logger.LogInformation("All Systems are healthy", r.Next());

        }
    }
}
