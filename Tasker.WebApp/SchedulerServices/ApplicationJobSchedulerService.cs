using System;
using Tasker.WebApp.Jobs;

namespace Tasker.WebApp.SchedulerServices
{
    public class ApplicationJobSchedulerService : JobSchedulerService
    {
        public ApplicationJobSchedulerService(IServiceProvider serviceProvider)
        {
            var Job = new CheckSystemHardwareJob(serviceProvider, TimeSpan.FromSeconds(30), "Hardware in excellent condition");
            Job.Executing += Job_Executing;

            AddJob(Job, new StartOnNextDayOfWeek() { DayOfWeek = DayOfWeek.Saturday, Hour = 15, Minute = 30, Second = 15 });

            //you can fetch jobs from db and add them here
        }

        private void Job_Executing(object sender, JobExecutingEventArgs e)
        {
            
        }
    }
}
