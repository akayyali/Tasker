using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tasker
{
    public class RunOn
    {
        
    }

    public abstract class JobSchedulerService : BackgroundService
    {
        protected readonly Dictionary<Timer, Job> JobsAndTimer = new();
        protected readonly Dictionary<Guid, JobStatusEnum> JobsStatus = new();

        public bool AddJob(Job job, bool startImmediatly = true)
        {

            if (JobsAndTimer.TryAdd(new Timer(job.Execute, job.Payload, startImmediatly ? TimeSpan.Zero : Timeout.InfiniteTimeSpan, startImmediatly ? job.RunEvery : Timeout.InfiniteTimeSpan), job))
            {
                JobsStatus.TryAdd(job.JobId, JobStatusEnum.Started);
                return true;
            }

            return false;
        }

        public bool AddJob(Job job, DateTime? startOn)
        {
            if (startOn.HasValue == false)
                startOn = DateTime.Now;

            if (startOn.Value < DateTime.Now)
                throw new ArgumentOutOfRangeException($"{nameof(startOn)} should be set to future date, or to NULL ");

            var waitBeforeStarting = startOn.Value - DateTime.Now;

            if (JobsAndTimer.TryAdd(new Timer(job.Execute, job.Payload, waitBeforeStarting , job.RunEvery), job))
            {
                JobsStatus.TryAdd(job.JobId, JobStatusEnum.Started);
                return true;
            }

            return false;
        }

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
        public IEnumerable<Job> Jobs
        {
            get
            {
                return JobsAndTimer.Values.Select(job => job);
            }
        }
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
