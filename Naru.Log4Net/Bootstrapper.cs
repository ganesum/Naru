using System;
using System.IO;

using ILogInject.Unity;

using Microsoft.Practices.Unity;

namespace Naru.Log4Net
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureNaruLog4Net(this IUnityContainer container, string sectionName)
        {
            container
                .AddNewExtension<BuildTracking>()
                .AddNewExtension<CommonLoggingLogCreationExtension>()
                .RegisterInstance<ILog4NetConfiguration>(new Log4NetConfiguration(sectionName));

            var configuration = container.Resolve<ILog4NetConfiguration>();
            if (!Directory.Exists(configuration.LogDirectoryPath))
            {
                Directory.CreateDirectory(configuration.LogDirectoryPath);
            }
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(configuration.LogDirectoryPath, configuration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            return container;
        }
    }
}