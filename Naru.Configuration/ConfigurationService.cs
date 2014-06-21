using System.Configuration;

namespace Naru.Configuration
{
    public interface IConfigurationService
    {
        T Get<T>();
    }

    public class ConfigurationService : IConfigurationService
    {
        public T Get<T>()
        {
            var section = (Configuration<T>)ConfigurationManager.GetSection(typeof(T).Name);

            return section == null
                       ? default(T)
                       : section.ConfigurationItem;
        }
    }
}