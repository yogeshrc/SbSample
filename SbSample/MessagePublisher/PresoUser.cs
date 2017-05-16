using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MessagePublisher
{
    [DataContract]
    internal class PresoUser
    {
        public PresoUser()
        {
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public object Role { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    enum Roles
    {
        Presenter
    }
}