using System;
using System.IO;
using Autofac;

namespace Naru.Log4Net
{
    public class Log4NetModule : Module
    {
        public string SectionName { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new Log4NetConfiguration(SectionName);

            if (!Directory.Exists(configuration.LogDirectoryPath))
            {
                Directory.CreateDirectory(configuration.LogDirectoryPath);
            }
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(configuration.LogDirectoryPath, configuration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        }
    }
}