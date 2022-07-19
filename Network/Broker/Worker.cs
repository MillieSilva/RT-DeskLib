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
            var brokerAddress = Constants.GetBrokerAddress();
            
            if (brokerAddress == null)
                return;

            client = new RestClient(brokerAddress)
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
