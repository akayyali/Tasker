using System;

namespace Tasker
{
    public abstract class Job
    {
        public Guid JobId = Guid.NewGuid();
        public object Payload { get; init; }
        public TimeSpan RunEvery { get; init; }
        public virtual void Execute(object payload) { }
        public Type JobType { get { return GetType(); } }
    }

}
