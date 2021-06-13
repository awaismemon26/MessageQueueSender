using Azure.Storage.Queues;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    public class AzureQueueSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AzureQueueSenderService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        private async Task SendMessage(string message)
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
    }
}
