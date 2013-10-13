using System.Threading.Tasks;

using Agatha.Common;

namespace Naru.Agatha
{
    public interface IRequestTask
    {
        Task<TResponse> Get<TRequest, TResponse>(TRequest request)
            where TRequest : Request<TResponse>
            where TResponse : Response;
    }
}