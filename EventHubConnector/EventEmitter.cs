
using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHubConnector
{
    public static class EventEmitter
    {
        [FunctionName("EventEmitter")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("Emit the EventHub messages");

                var number = int.Parse(req.Query["number"]);

                await SendMessage(number);

                return new OkObjectResult($"{number} Messages has been sent.");
            }
            catch (Exception e)
            {
                if (e is FormatException || e is ArgumentException)
                {
                    return new BadRequestObjectResult($"Input parameter is wrong. Please specify ?number=YOUR_NUMBER (Duration = YOUR_NUMBER x 10 millisec. e.g. ?number=1000) \nException: {e.Message}");

                }
                else
                {
                    return new ExceptionResult(e, true);
                }
            }
        }

        private static EventHubClient _client;
        static EventEmitter() { 
            var conenctionStringBuilder = new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("EventHubConnectionString"));
            _client = EventHubClient.CreateFromConnectionString(conenctionStringBuilder.ToString());
        }

        private static async Task SendMessage(int messageToSend)
        {
            for (var i = 0; i < messageToSend; i++)
            {
                var message = SimpleMessage.RandomSimpleMessage().ToString();
                await _client.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                await Task.Delay(10);
            }
        }

        private class SimpleMessage

        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public static SimpleMessage RandomSimpleMessage()
            {
                return new SimpleMessage()
                {
                    Id = Guid.NewGuid().ToString(),
                    Body = GetRandomMessage()
                };
            }

            private static string[] messages = new string[]
            {
                "Hi, I'm a ninja.",
                "I work on Microsoft.",
                "Ken has some good muscle.",
                "Yoichi is a great hacker."
            };

            private static Random rand = new Random();

            private static string GetRandomMessage()
            {
                var index = rand.Next(messages.Length);
                return messages[index];
            }
            
        }

    }
}
