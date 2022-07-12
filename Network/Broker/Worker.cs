// System Imports

// Application Imports

// External Imports
using Newtonsoft.Json;
using RestSharp;


namespace Library.Network.Broker
{
    public class BrokerWorkerRestClient
    {
        private string Route = "api/v1/worker";

        RestClient client { get; }


        public BrokerWorkerRestClient()
        {
            // TODO: Correct broker metadata hostnames to proper API urls
            // var broker_address = Metadata.GetValidBrokerAddress();

            var broker_address = "http://127.0.0.1:5000";

            if (broker_address == null)
                return;

            client = new(broker_address)
            {
                // Authenticator = new HttpBasicAuthenticator("rt", "dev")
            };
            client.Options.ThrowOnAnyError = true;
            client.Options.MaxTimeout = 5000;
        }

        public async Task<Guid> Register(Guid uuid)
        {
            var request = new RestRequest($"/{Route}/registry/register");

            var response = await client.PostAsync<Guid>(request);

            return response;
        }

        public async Task<Guid> Synchronize(string hwid)
        {
            var request = new RestRequest($"/{Route}/registry/synchronize{hwid}");

            var response = client.Post(request);
            var synchronization = JsonConvert.DeserializeObject<SynchronizationResponse>(response.Content);

            return synchronization.UUID;
        }
    }

    public struct SynchronizationResponse
    {
        public Guid UUID;
    }
}
