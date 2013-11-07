using System.Runtime.Serialization;

namespace Naru.Agatha
{
    [DataContract]
    public abstract class Request : global::Agatha.Common.Request
    {
        [DataMember]
        public string Id { get; set; }
    }
}
