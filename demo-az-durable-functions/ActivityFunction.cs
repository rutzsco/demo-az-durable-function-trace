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
    public class HelloActivityFunction
    {
        private readonly TelemetryClient telemetryClient;

        /// Using dependency injection will guarantee that you use the same configuration for telemetry collected automatically and manually.
        public HelloActivityFunction(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        [FunctionName("HelloActivity")]
        public string SayHello([ActivityTrigger] IDurableActivityContext ctx, ILogger log)
        {
            log.LogInformation($"DurableInstanceId {ctx.InstanceId}");
            var name = ctx.GetInput<string>();
            Activity.Current.AddBaggage("CUSTOM-PROPERTY2", "MY VALUE");
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }
    }
}