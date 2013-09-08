using System.Threading.Tasks;

using Naru.WPF.TPL;

namespace Naru.WPF.Tests.TPL
{
    public class TestTaskScheduler : ITaskScheduler
    {
        public TaskScheduler Default { get; private set; }

        public TestTaskScheduler()
        {
            Default = new CurrentThreadTaskScheduler();
        }
    }
}