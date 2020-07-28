using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace demo_az_durable_functions
{
    public static class TriggerFunction
    {
        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
                                                                [DurableClient] IDurableOrchestrationClient starter,
                                                                ILogger log)
        {
            // Function input comes from the request content.

            Activity.Current.AddTag("CUSTOM-PROPERTY", "MY VALUE");

            string instanceId = await starter.StartNewAsync("WorkflowFunction", null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}