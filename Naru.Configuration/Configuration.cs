using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Naru.Configuration
{
    public abstract class Configuration<T> : ConfigurationSection
        where T : Configuration<T>
    {
        private XmlSerializer _serializer;

        public T ConfigurationItem { get; private set; }

        protected override void Init()
        {
            base.Init();

            var root = new XmlRootAttribute(SectionInformation.Name);
            _serializer = new XmlSerializer(typeof (T), root);
        }

        protected override void DeserializeSection(XmlReader reader)
        {
            ConfigurationItem = (T) _serializer.Deserialize(reader);
        }
    }
}