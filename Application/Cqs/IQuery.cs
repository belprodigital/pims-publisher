using MediatR;

namespace PimsPublisher.Application.Cqs
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}
