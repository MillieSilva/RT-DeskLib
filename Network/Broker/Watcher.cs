// System Imports
using System.Collections.ObjectModel;

// Library Imports

// External Imports
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;


namespace Library.Network.Broker
{
    public class BrokerWatcherRestClient
    {
        private string Route = "api/v1/watcher";

        RestClient client { get; }
        
        public bool Available { get; private set; }
        public string Address => client.Options.BaseUrl.ToString();


        public BrokerWatcherRestClient()
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

            Available = IsAvailable();
        }

        internal bool IsAvailable()
        {
            try
            {
                var request = new RestRequest($"api/v1/check_status");

                var response = client.Execute(request);
                if (response.ErrorException != null)
                {
                    throw response.ErrorException;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ObservableCollection<Guid>> RequestAllWorkerIds()
        {
            var request = new RestRequest($"/{Route}/discovery/discover_all");

            var response = client.Get(request);
            var workerIds = JsonConvert.DeserializeObject<WorkerIds>(response.Content);

            // FIXME: For some odd reason GetAsync with/without a given type is processing forever here,
            //        but above works fine which is technically the identical approach but without async

            // var workersMetadata = await client.GetAsync<List<Guid>>(request);

            return workerIds.Ids;
        }

        public async Task<WorkerMetadata> RequestWorkerMetadata(Guid uuid)
        {
            var request = new RestRequest($"/{Route}/discovery/discover{uuid}");

            var response = client.Get(request);
            var workerMetadata = JsonConvert.DeserializeObject<WorkerMetadata>(response.Content);

            return workerMetadata;
        }
    }
}


public struct WorkerIds
{
    public ObservableCollection<Guid> Ids;
}

public struct WorkerMetadata
{
    public Guid UUID;
    public ObservableCollection<string> Public_IPV4;
}
