using System.Threading.Tasks;

namespace Naru.WPF.TPL
{
    public interface ITaskScheduler
    {
        TaskScheduler Default { get; }
    }
}