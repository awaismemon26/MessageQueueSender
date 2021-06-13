using Azure.Storage.Queues.Models;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IAzureQueueSenderService _queueService;

        public QueueController(IAzureQueueSenderService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<QueueMessage[]> GetAllMessagesFromQueue()
        {
            return await _queueService.RetrieveAllMessages();
        } 

        [HttpPost]
        public async Task PostMessage([FromQuery] string message)
        {
            if (string.IsNullOrEmpty(message))
                BadRequest();
            string messageTrimmed = message.Trim('"');
            await _queueService.SendMessage(messageTrimmed);
        }
    }
}
