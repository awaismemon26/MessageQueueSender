using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueueSenderTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMessageSender _messageSender;

        public WeatherForecastController(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        [HttpPost]
        public async Task PostMessage()
        {
            await _messageSender.Send("Hello World");
        }
    }
}
