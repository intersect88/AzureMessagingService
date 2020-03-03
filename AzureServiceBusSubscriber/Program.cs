using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusSubscriber
{
    class Program
    {
        const string AzureServiceBusConnectionString = "AzureServiceBusConnectionString";
        const string AzureServiceBusTopic = "demotopic";
        const string AzureServiceBusSubscription = "demoTopicSubscription";
        static ISubscriptionClient subscriptionClient;

        static async Task Main(string[] args)
        {
            subscriptionClient = new SubscriptionClient(AzureServiceBusConnectionString, AzureServiceBusTopic, AzureServiceBusSubscription);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1,
                
            };

            subscriptionClient.RegisterMessageHandler(
                ProcessMessages, messageHandlerOptions
            );

            Console.ReadKey();

            await subscriptionClient.CloseAsync();

        }

        static async Task ProcessMessages(Message message, CancellationToken token)
        {
            string messageReceived = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine("Received -> " + messageReceived);
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Exception: {exceptionReceivedEventArgs.Exception}");
            return Task.CompletedTask;
        }
    }
}
