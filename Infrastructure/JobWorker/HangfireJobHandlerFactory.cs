using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PimsPublisher.Infrastructure.JobWorker
{
    internal class HangfireJobHandlerFactory : IJobFactory
    {
        IJobHandler IJobFactory.Create<TJMessage>()
        {
            throw new NotImplementedException();
        }
    }
}
