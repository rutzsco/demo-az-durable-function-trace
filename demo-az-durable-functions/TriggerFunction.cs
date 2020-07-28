using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace demo_az_durable_functions
{
    public class TriggerFunction
    {
        private readonly TelemetryClient telemetryClient;

        /// Using dependency injection will guarantee that you use the same configuration for telemetry collected automatically and manually.
        public TriggerFunction(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        [FunctionName("Function1_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
                                                                [DurableClient] IDurableOrchestrationClient starter,
                                                                ILogger log)
        {
            // Function input comes from the request content.

            Activity.Current.AddTag("CUSTOM-PROPERTY", "MY VALUE");
            telemetryClient.Context.GlobalProperties.Add("CUSTOM-PROPERTY-TC1", "MY VALUE");

            string instanceId = await starter.StartNewAsync("WorkflowFunction", null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            telemetryClient.TrackTrace($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}