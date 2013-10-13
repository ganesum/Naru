using System.Runtime.Serialization;

namespace Naru.Agatha
{
    [DataContract]
    public abstract class Request<TResponse> : global::Agatha.Common.Request
        where TResponse : global::Agatha.Common.Response
    {
        [DataMember]
        public string Id { get; set; }
    }
}