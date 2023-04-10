using MediatR;

namespace PimsPublisher.Application.Cqs
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {}
}