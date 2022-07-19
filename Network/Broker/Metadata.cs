// System Imports

// Library Imports

// External Imports
using Newtonsoft.Json;
using ServiceStack;



namespace Library.Network.Broker
{
    internal static class Metadata
    {
        public static List<string> BrokerSources = new()
        {
            "https://barakat2.github.io/RT-Meta/broker.json",

        };

        public static Uri? GetValidBrokerAddress()
        {
            foreach (var address in BrokerSources)
            {
                string? metadataJson;
                try
                {
                    metadataJson = address.GetJsonFromUrl();
                }
                catch (Exception)
                {

                    return null;
                }

                var metadata = JsonConvert.DeserializeObject<BrokerMetadata>(metadataJson);

                var uriBuilder = new UriBuilder(metadata.Hostnames[0]);

                return uriBuilder.Uri;
            }

            return null;
        }
    }

    internal struct BrokerMetadata 
    {
        public List<string> Hostnames;
    }
}
