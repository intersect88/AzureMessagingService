using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubsReceiver
{
    class Program
    {
        const string EventHubsConnectionString = "EventHubsConnectionString";
        const string EventHub = "eventhubdemo";
        const string AzureBlobStorageConnectionString = "AzureBlobStorageConnectionString";
        const string AzureBlobStorageContainer = "eventhubcontainer";
        public static async Task Main(string[] args)
        {
            string eventHubConsumerClient = EventHubConsumerClient.DefaultConsumerGroupName;

            BlobContainerClient blobContainerClient = new BlobContainerClient(AzureBlobStorageConnectionString, AzureBlobStorageContainer);

            EventProcessorClient eventProcessorClient = new EventProcessorClient(blobContainerClient, eventHubConsumerClient, EventHubsConnectionString, EventHub);
            eventProcessorClient.ProcessEventAsync += ProcessingEvent;
            eventProcessorClient.ProcessErrorAsync += ProcessingError;

            await eventProcessorClient.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(10));

            await eventProcessorClient.StopProcessingAsync();

        }

        private static Task ProcessingEvent (ProcessEventArgs processEventArgs) 
        {
            Console.WriteLine("Received -> " + Encoding.UTF8.GetString(processEventArgs.Data.Body.ToArray()));
            return Task.CompletedTask;
        }

        private static Task ProcessingError(ProcessErrorEventArgs processErrorEventArgs)
        {
            Console.WriteLine(processErrorEventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
