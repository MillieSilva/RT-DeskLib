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
                string? metadata_json = null;
                try
                {
                    metadata_json = address.GetJsonFromUrl();
                }
                catch (Exception)
                {

                    return null;
                }

                var metadata = JsonConvert.DeserializeObject<BrokerMetadata>(metadata_json);

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
