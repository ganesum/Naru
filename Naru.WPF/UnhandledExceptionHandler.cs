using System.Windows.Threading;

using Common.Logging;

namespace Naru.WPF
{
    public static class UnhandledExceptionHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Naru.WPF.UnhandledExceptionHandler));

        public static void InstallDispatcherUnhandledException()
        {
            Logger.Debug("Installing DispatcherUnhandledException logging");

            Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_UnhandledException;
        }

        public static void UninstallDispatcherUnhandledException()
        {
            Logger.Debug("Uninstalling DispatcherUnhandledException logging");

            Dispatcher.CurrentDispatcher.UnhandledException -= CurrentDispatcher_UnhandledException;
        }

        private static void CurrentDispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception);
        }
    }
}