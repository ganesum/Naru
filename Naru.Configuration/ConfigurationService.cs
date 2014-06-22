using System;
using System.Configuration;

using Common.Logging;

namespace Naru.Configuration
{
    public interface IConfigurationService
    {
        T Get<T>();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly ILog _log;

        public ConfigurationService(ILog log)
        {
            _log = log;
        }

        public T Get<T>()
        {
            var sectionName = typeof (T).Name;

            try
            {
                _log.DebugFormat("Attempting to load configuration section {0}", sectionName);

                var section = (Configuration<T>)ConfigurationManager.GetSection(sectionName);

                var configuration = section == null
                                        ? default(T)
                                        : section.ConfigurationItem;

                _log.DebugFormat("Successfully loaded configuration section {0}", sectionName);

                return configuration;
            }
            catch (Exception exception)
            {
                _log.ErrorFormat("Failed to load configuration section {0}", exception, sectionName);
                
                throw;
            }
        }
    }
}