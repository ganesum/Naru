using System.Threading.Tasks;

using Common.Logging;

namespace Naru.TPL
{
    public static class UnhandledExceptionHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UnhandledExceptionHandler));

        public static void InstallTaskUnobservedException()
        {
            Logger.Debug("Installing UnobservedTaskException logging");

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        public static void UninstallTaskUnobservedException()
        {
            Logger.Debug("Uninstalling UnobservedTaskException logging");

            TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
        }

        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();

            var exception = e.Exception.Flatten();

            Logger.Error(exception);
        }
    }
}