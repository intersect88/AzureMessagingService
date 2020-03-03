using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusPublisher
{
    class Program
    {
        const string AzureServiceBusConnectionString = "AzureServiceBusConnectionString";
        const string AzureServiceBusTopic = "demotopic";
        static ITopicClient topicClient;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Type the number of messages that you want to publish -> ");
            int numberOfMessage = int.Parse(Console.ReadLine());
            topicClient = new TopicClient(AzureServiceBusConnectionString, AzureServiceBusTopic);

            for (int i = 0; i < numberOfMessage; i++)
            {
                string messageBody = $"Demo Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await topicClient.SendAsync(message);
            }
            await topicClient.CloseAsync();
        }
    }
}
