using Azure.Storage.Queues.Models;

using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    public interface IAzureQueueSenderService
    {
        Task DequeueMessages(string queueName);
        Task<QueueMessage[]> RetrieveAllMessages();
        Task SendMessage(string message);
    }
}