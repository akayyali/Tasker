using System;
using System.Threading;

namespace Tasker
{
    public class JobExecutingEventArgs
    {
        public bool Skip { get; set; }
        public Guid ExecutionId { get; set; }
    }
    public class JobExecutedEventArgs
    {
        public Guid ExecutionId { get; set; }

    }
    public abstract partial class Job
    {
        public event EventHandler<JobExecutingEventArgs> Executing;
        public event EventHandler<JobExecutedEventArgs> Executed;

        bool _skipExecution;
        Guid _executionId = Guid.Empty;
        public Job(TimeSpan runEvery , object payload )
        {
            RunEvery = runEvery;
            Payload = payload;

        }

        /// <summary>
        /// Auto generated Id, to help track Jobs, this is useful when you want to stop or remove an existing Job, and as a correlation Id 
        /// </summary>
        public Guid JobId = Guid.NewGuid();

        /// <summary>
        /// a data bag to be used carefully
        /// </summary>
        public object Payload { get; private set; }
        public TimeSpan RunEvery { get; private set; }
        internal void Execute(object payload)
        {

            if (!_skipExecution)
            {
                ExecuteJob(payload);
            }

        }
        public virtual void ExecuteJob(object payload) { }
        public Type JobType { get { return GetType(); } }

        //The event-invoking method that derived classes can override.
        internal void OnExecuting(JobExecutingEventArgs e)
        {
            // Safely raise the event for all subscribers
            _executionId = Guid.NewGuid();
            e.ExecutionId = _executionId;
            Executing?.Invoke(this, e);
            _skipExecution = e.Skip;
        }

        internal void OnExecuted(JobExecutedEventArgs e)
        {
            // Safely raise the event for all subscribers
            e.ExecutionId = _executionId;
            if (!_skipExecution)
                Executed?.Invoke(this, e);
        }
    }

}
