using MessagePublisher;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MessageSubscriber
{
    class Program
    {
        private static readonly string Subscription = "ConsoleSub";
        private static readonly string Topic = "PresoTopic";

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;

            bool continueReading = false;
            do
            {
                Console.WriteLine("Press <ENTER> to read message");
                Console.ReadLine();

                var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                if (!namespaceManager.SubscriptionExists(Topic, Subscription))
                    namespaceManager.CreateSubscription(Topic, Subscription);
                var client = SubscriptionClient.CreateFromConnectionString(connectionString, Topic, Subscription);
                client.OnMessageAsync(Callback);
                Console.WriteLine("Do you want to exit? (Y/N)");
                continueReading = Console.ReadKey().Key == ConsoleKey.N;
                Console.Clear();
            } while (continueReading);
        }

        private static Task Callback(BrokeredMessage message)
        {
            Console.WriteLine();
            Console.WriteLine("Received message...");
            Console.WriteLine("\t Message ID >> {0}", message.MessageId);
            var stream = message.GetBody<Stream>();
            using (var sr = new StreamReader(stream))
            {
                string messageText = sr.ReadToEnd();
                var user = new PresoUser(messageText);
                Console.WriteLine("\t Message Body >> \n{0}\n", messageText);
            }
            return Task.Run(() => message.Complete());
        }
    }
}
