using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PimsPublisher.Infrastructure.MediatorPipeline
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection TransactionBehavior(this IServiceCollection services)
            => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
    }
}
