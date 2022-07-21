// System Imports
using System.Threading.Tasks;

// Library Imports
using Library.Network.Broker;

// External Imports
using Xunit;


namespace Tests
{
    public class Broker
    {
        [Fact]
        public async Task<BrokerWatcherRestClient> TestBrokerWatcher()
        {
            var watcher = new BrokerWatcherRestClient();

            // var workers_metadata = await watcher.RequestAllWorkerIds();
            
            // Assert.True(workers_metadata.Id != null);

            return watcher;
        }

        /*
        [Fact]
        public async Task<BrokerWorkerRestClient> TestBrokerWorker()
        {

            var worker = new BrokerWorkerRestClient();
            Assert.True(await worker.Synchronize());

            return worker;
        }
        */
    }
}
