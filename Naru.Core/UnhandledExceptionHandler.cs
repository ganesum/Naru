using System;

using Common.Logging;

namespace Naru.Core
{
    public static class UnhandledExceptionHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UnhandledExceptionHandler));

        public static void InstallDomainUnhandledException()
        {
            Logger.Debug("Installing Domain UnhandledException logging");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void UninstallDomainUnhandledException()
        {
            Logger.Debug("Uninstalling Domain UnhandledException logging");

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ExceptionObject);
        }
    }
}