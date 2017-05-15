using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.IO;

namespace MessagePublisher
{
    class Program
    {
        private static readonly string Topic = "PresoTopic";

        static void Main(string[] args)
        {
            //Step 1: Create Topic
            string connectionString = 
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists(Topic))
                namespaceManager.CreateTopic(Topic);

            //Step 2: Build the object to serialize
            var user = new PresoUser() { Id = 1, Name = "John Doe", Role = Roles.Presenter };

            //Step 3: Create JSON message
            var jsonStream = new MemoryStream();
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(jsonStream))
                using (var jsonWriter = new JsonTextWriter(sw))
                    serializer.Serialize(jsonWriter, user);

            //Step 4: Send message
            var message = new BrokeredMessage(jsonStream);
            var client = TopicClient.CreateFromConnectionString(connectionString, Topic);
            client.Send(message);
        }
    }
}
