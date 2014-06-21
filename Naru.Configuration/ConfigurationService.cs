using System.Configuration;

namespace Naru.Configuration
{
    public interface IConfigurationService
    {
        T Get<T>()
            where T : Configuration<T>;
    }

    public class ConfigurationService : IConfigurationService
    {
        public T Get<T>()
            where T : Configuration<T>
        {
            var section = (T) ConfigurationManager.GetSection(typeof (T).Name);

            return section == null
                       ? null
                       : section.ConfigurationItem;
        }
    }
}