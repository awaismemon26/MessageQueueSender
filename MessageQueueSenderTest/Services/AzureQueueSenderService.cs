using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    public class AzureQueueSenderService : IAzureQueueSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureQueueSenderService> _logger;

        public AzureQueueSenderService(IConfiguration configuration, ILogger<AzureQueueSenderService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        public async Task SendMessage(string message)
        {
            var connStr = _configuration.GetValue<string>("AzureConfigurations:StorageQueue:ConnectionString");
            var storageQueueName = _configuration.GetValue<string>("AzureConfigurations:StorageQueue:Name");

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connStr, storageQueueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                await queueClient.SendMessageAsync(message);
            }

            _logger.LogInformation("Message: {Message} sent to queue {Queue}", message, storageQueueName);
        }

        public async Task<QueueMessage[]> RetrieveAllMessages()
        {
            var connStr = _configuration.GetValue<string>("AzureConfigurations:StorageQueue:ConnectionString");
            var storageQueueName = _configuration.GetValue<string>("AzureConfigurations:StorageQueue:Name");

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connStr, storageQueueName);

            var output = await queueClient.ReceiveMessagesAsync();
            _logger.LogInformation("Message recieved from Queue {Queue}", storageQueueName);

            return output;
        }

        //-----------------------------------------------------
        // Process and remove multiple messages from the queue
        //-----------------------------------------------------
        public async Task DequeueMessages(string queueName)
        {
            // Get the connection string from app settings
            var connStr = _configuration.GetValue<string>("AzureConfigurations:StorageQueue:ConnectionString");

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connStr, queueName);

            if (queueClient.Exists())
            {
                // Receive and process 20 messages
                QueueMessage[] receivedMessages = await queueClient.ReceiveMessagesAsync(20, TimeSpan.FromMinutes(5));

                foreach (QueueMessage message in receivedMessages)
                {
                    // Process (i.e. print) the messages in less than 5 minutes
                    Console.WriteLine($"De-queued message: '{message.MessageText}'");

                    // Delete the message
                    queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                }
            }
        }
    }
}
