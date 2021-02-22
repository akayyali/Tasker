using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasker.WebApp.Jobs
{
    public class ApplicationJobSchedulerService : JobSchedulerService
    {
        public ApplicationJobSchedulerService(IServiceProvider serviceProvider)
        {
            var Job = new CheckSystemHardwareJob(serviceProvider)
            {
                RunEvery = TimeSpan.FromSeconds(10),
                Payload = "Hardware in excellent condition"
            };
            AddJob(Job, DateTime.Now.AddSeconds(30));

            //you can fetch jobs from db and add them here
        }
    }
}
