using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    public interface IAzureQueueSenderService
    {
        Task SendMessage(string message);
    }
}