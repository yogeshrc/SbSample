using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Communication.Core
{
    public class ServiceBusEventListener<TJsonMessageType> 
        : IMessageListener<TJsonMessageType>
        where TJsonMessageType : class, new()
    {
        private const string SbConnectionStringKey = "ConnectionString";
        private const string SbSubscriberNameKey = "SubscriberName";
        private const string SbTopicKey = "TopicName";

        public event EventHandler<MessageReceivedEventArgs<TJsonMessageType>> MessageReceived;

        public void Initialize(Dictionary<string, dynamic> configurations)
        {
            ValidateConfiguration(configurations);

            string connectionString = configurations[SbConnectionStringKey];
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            string topic = configurations[SbTopicKey];
            string subscriber = configurations[SbSubscriberNameKey];
            if (!namespaceManager.SubscriptionExists(topic, subscriber))
                namespaceManager.CreateSubscription(topic, subscriber);
            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topic, subscriber);
            client.OnMessageAsync(Callback);
        }

        private Task Callback(BrokeredMessage message)
        {
            string jsonText = string.Empty;
            var stream = message.GetBody<Stream>();
            using (var reader = new StreamReader(stream))
                jsonText = reader.ReadToEnd();

            MessageReceived?.Invoke(this, new MessageReceivedEventArgs<TJsonMessageType>()
            {
                Message = JsonConvert.DeserializeObject<TJsonMessageType>(jsonText)
            });

            return Task.Run(() => message.Complete());
        }

        private void ValidateConfiguration(Dictionary<string, dynamic> configurations)
        {
            if (!(configurations.ContainsKey(SbConnectionStringKey) && 
                    configurations.ContainsKey(SbTopicKey)) &&
                    configurations.ContainsKey(SbSubscriberNameKey))
                throw new ArgumentException("Missing configuration!!! Expected ConnectionString, Topic subscribed, Subscriber name");
        }
    }
}
