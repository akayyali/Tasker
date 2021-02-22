using System;

namespace Tasker
{
    public abstract class Job
    {
        /// <summary>
        /// Auto generated Id, to help track Jobs, this is useful when you want to stop or remove and existing Job, and for correlation 
        /// </summary>
        public Guid JobId = Guid.NewGuid();
        
        /// <summary>
        /// a data bag to be used carefully
        /// </summary>
        public object Payload { get; init; }
        public TimeSpan RunEvery { get; init; }
        public virtual void Execute(object payload) { }
        public Type JobType { get { return GetType(); } }
    }

}
