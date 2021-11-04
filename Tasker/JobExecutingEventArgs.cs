using System;

namespace Tasker
{
    public class JobExecutingEventArgs
    {
        public bool Skip { get; set; }
        public Guid ExecutionId { get; set; }
    }

}
