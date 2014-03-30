using System;
using System.Threading.Tasks;

using Agatha.Common;

using Common.Logging;

using Naru.Concurrency.Scheduler;
using Naru.Core;
using Naru.TPL;
using Naru.WPF.Scheduler;

namespace Naru.Agatha
{
    public class RequestTask : IRequestTask
    {
        private readonly ILog _log;
        private readonly Func<IRequestDispatcher> _requestDispatcherFactory;
        private readonly ISchedulerProvider _schedulerProvider;

        public RequestTask(ILog log, Func<IRequestDispatcher> requestDispatcherFactory, ISchedulerProvider schedulerProvider)
        {
            _log = log;
            _requestDispatcherFactory = requestDispatcherFactory;
            _schedulerProvider = schedulerProvider;
        }

        public Task<TResponse> Get<TResponse>(Request<TResponse> request) 
            where TResponse : Response
        {
            return Task.Factory.StartNew(() => Execute(request), _schedulerProvider.Task.TPL);
        }

        private TResponse Execute<TResponse>(Request<TResponse> request)
            where TResponse : Response
        {
            using (var performanceTester = new PerformanceTester())
            {
                request.Id = Guid.NewGuid().ToString();

                _log.Debug(string.Format("Start RequestTask {0}, Id - {1}", request.GetType(), request.Id));

                using (var requestDispatcher = _requestDispatcherFactory())
                {
                    var response = requestDispatcher.Get<TResponse>(request);
    
                    if (response.Exception != null)
                        throw new RequestException(response.Exception.Message);
    
                    _log.Debug(string.Format("Finished RequestTask {0}, Id - {1}. Duration {2}",
                        request.GetType(),
                        request.Id,
                        performanceTester.Result.Milliseconds));
    
                    return response;
                }
            }
        }
    }
}