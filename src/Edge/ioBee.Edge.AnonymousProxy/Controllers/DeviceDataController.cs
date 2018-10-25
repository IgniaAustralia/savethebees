using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices.Client;

namespace ioBee.Edge.AnonymousProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDataController : ControllerBase
    {
        private DeviceClient _deviceClient;

        public DeviceDataController(DeviceClient deviceClient)
        {
            _deviceClient = deviceClient;
        }
        // GET api/values
        /// <summary>
        /// Dummy method for sending messages to the hub, used only for debugging
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var temperature = 123;
            var humidity = 32;
            var dataBuffer = $"{{\"messageId\":11,\"temperature\":{temperature},\"humidity\":{humidity}}}";

            await SendEvent(dataBuffer);
            return Ok();
        }

        // POST api/values
        /// <summary>
        /// Allows us to forward an un-authed message onto the iothub, the auth is handled by this web app
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] dynamic value)
        {
            var stringValue = (string)value.ToString();
            await SendEvent(stringValue);
            return Ok();
        }
        private async Task SendEvent(string json)
        {
            Console.WriteLine("Device sending a messages to IoTHub...\n");


            Message eventMessage = new Message(Encoding.UTF8.GetBytes(json));
            //eventMessage.Properties.Add("temperatureAlert", (_temperature > TemperatureThreshold) ? "true" : "false");
            Console.WriteLine("\t{0}> Sending message:, Data: [{1}]", DateTime.Now.ToLocalTime(), json);

            await _deviceClient.SendEventAsync(eventMessage).ConfigureAwait(false);
        }
    }
}
