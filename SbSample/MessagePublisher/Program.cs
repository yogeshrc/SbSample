using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

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

            //Step 3: Create to JSON stream
            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(user.ToString()));

            //Step 4: Build message to send
            var message = new BrokeredMessage(jsonStream);

            //Step 5: Send the message
            var client = TopicClient.CreateFromConnectionString(connectionString, Topic);
            Console.WriteLine("About to send message. Press <ENTER> to continue");
            Console.ReadLine();
            client.Send(message);

            Console.WriteLine("Message sent. Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}
