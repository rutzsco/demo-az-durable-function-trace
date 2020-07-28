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
    public static class WorkflowFunction
    {
        [FunctionName("WorkflowFunction")]
        public static async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var outputs = new List<string>();

            Activity.Current.AddTag("CUSTOM-PROPERTY", "MY VALUE");
            log.LogInformation($"Running Workflow");

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("HelloActivity", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("HelloActivity", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("HelloActivity", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("HelloActivity")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }
    }
}