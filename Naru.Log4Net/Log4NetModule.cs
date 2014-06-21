using System;
using System.IO;
using System.Linq;

using Autofac;
using Autofac.Core;

using Common.Logging;

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

        protected override void AttachToComponentRegistration(IComponentRegistry registry,
                                                              IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var limitType = e.Component.Activator.LimitType;

            var resolvedParameter = new ResolvedParameter((p, i) => p.ParameterType == typeof (ILog),
                                                          (p, i) => LogManager.GetLogger(limitType));

            e.Parameters = e.Parameters.Union(new[] {resolvedParameter});
        }
    }
}