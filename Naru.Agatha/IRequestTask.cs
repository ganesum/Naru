using System.Threading.Tasks;

using Agatha.Common;

namespace Naru.Agatha
{
    public interface IRequestTask
    {
        Task<TResponse> Get<TResponse>(Request<TResponse> request)
            where TResponse : Response;
    }
}
