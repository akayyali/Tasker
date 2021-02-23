using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tasker
{
    
    public abstract class JobSchedulerService : BackgroundService
    {
        protected readonly Dictionary<Timer, Job> JobsAndTimer = new();
        protected readonly Dictionary<Guid, JobStatusEnum> JobsStatus = new();

        /// <summary>
        /// Adds a new recurring Job
        /// </summary>
        /// <param name="job">The job to be run on each interval</param>
        /// <param name="startOn">Set it to a week day to schedule the run to the next day of week, you can also set a specfic time </param>
        /// <returns></returns>
        public bool AddJob(Job job, StartOnNextDayOfWeek startOn)
        {
            
            var firstRunOn = (DateTime.Now.Date + new TimeSpan(startOn.Hour, startOn.Minute, startOn.Second)).
                GetNextWeekday(startOn.DayOfWeek);

            return AddJob(job, firstRunOn);
        }

        /// <summary>
        /// Adds a new recurring Job
        /// </summary>
        /// <param name="job">The job to be run on each interval</param>
        /// <param name="startOn">set to false if you don`t want to start the job immediatly, default is true</param>
        /// <returns></returns>
        public bool AddJob(Job job, bool startImmediatly = true)
        {

            if (JobsAndTimer.TryAdd(new Timer(job.Execute, job.Payload, startImmediatly ? TimeSpan.Zero : Timeout.InfiniteTimeSpan, startImmediatly ? job.RunEvery : Timeout.InfiniteTimeSpan), job))
            {
                JobsStatus.TryAdd(job.JobId, JobStatusEnum.Started);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new recurring Job
        /// </summary>
        /// <param name="job">The job to be run on each interval</param>
        /// <param name="startOn">When to start the Job, set it to NULL if you want start immediatly</param>
        /// <returns></returns>
        public bool AddJob(Job job, DateTime? startOn)
        {
            if (startOn.HasValue == false)
                startOn = DateTime.Now;

            if (startOn.Value < DateTime.Now)
                throw new ArgumentOutOfRangeException($"{nameof(startOn)} should be set to future date, or to NULL ");

            var waitBeforeStarting = startOn.Value - DateTime.Now;

            if (JobsAndTimer.TryAdd(new Timer(job.Execute, job.Payload, waitBeforeStarting, job.RunEvery), job))
            {
                JobsStatus.TryAdd(job.JobId, JobStatusEnum.Started);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Stops a Job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool RemoveJob(Guid jobId)
        {
            bool deleted = false;
            var jobToBeDeleteds = JobsAndTimer.Where(x => x.Value.JobId == jobId).ToList();

            foreach (var job in jobToBeDeleteds)
            {
                StopJob(job.Value.JobId);
                JobsAndTimer.Remove(job.Key);
                JobsStatus.Remove(job.Value.JobId);
                deleted = true;
            }

            return deleted;
        }

        /// <summary>
        /// returns an IEnumerable of all registered Jobs
        /// </summary>
        public IEnumerable<Job> Jobs
        {
            get
            {
                return JobsAndTimer.Values.Select(job => job);
            }
        }

        /// <summary>
        /// Stops a Job from running, if the job is already started, current execution will continue but further runs won`t happen
        /// </summary>
        /// <param name="JobId"></param>
        public void StopJob(Guid JobId)
        {
            foreach (var job in JobsAndTimer)
            {
                if (job.Value.JobId == JobId)
                {
                    job.Key.Change(Timeout.Infinite, Timeout.Infinite);
                    JobsStatus[JobId] = JobStatusEnum.Stopped;
                }
            }
        }

        /// <summary>
        /// Starts a job that is registered
        /// </summary>
        /// <param name="JobId"></param>
        public void StartJob(Guid JobId)
        {
            foreach (var job in JobsAndTimer)
            {
                if (job.Value.JobId == JobId)
                {
                    job.Key.Change(TimeSpan.Zero, job.Value.RunEvery);
                    JobsStatus[JobId] = JobStatusEnum.Started;
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
