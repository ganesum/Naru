using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    public class DesktopTaskScheduler : ITaskScheduler
    {
        public TaskScheduler Default { get; private set; }

        public DesktopTaskScheduler()
        {
            Default = TaskScheduler.Default;
        }
    }
}