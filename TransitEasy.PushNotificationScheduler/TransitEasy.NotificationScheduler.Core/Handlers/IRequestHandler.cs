using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Handlers
{
    public interface IRequestHandler<TRequest, TResult>
    {
        Task<TResult> HandleRequest(TRequest request);
    }
}
