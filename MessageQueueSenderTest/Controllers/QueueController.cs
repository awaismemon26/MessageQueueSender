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
        public ActionResult GetAllMessagesFromQueue()
        {
            return Ok();
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
