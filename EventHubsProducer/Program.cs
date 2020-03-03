using Azure.Messaging.EventHubs.Producer;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubsProducer
{
    class Program
    {
        const string EventHubsConnectionString = "EventHubsConnectionString";
        const string EventHub = "eventhubdemo";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Type the number of events that you want to publish -> ");
            int numberOfEvent = int.Parse(Console.ReadLine());


            await using var eventHubProducerClient = new EventHubProducerClient(EventHubsConnectionString, EventHub);
            using EventDataBatch eventDataBatch = await eventHubProducerClient.CreateBatchAsync();

            for (int i = 0; i < numberOfEvent; i++)
            {
                string eventBody = $"Demo Message {i}";
                eventDataBatch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(eventBody)));
            }

            await eventHubProducerClient.SendAsync(eventDataBatch);
        }
    }
}
