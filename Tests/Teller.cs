// System Imports

// Library Imports
using Library.Network;
using Library.Network.Teller;

// External Imports
using Xunit;


namespace Tests
{
    public class Teller
    {
        [Fact]
        public TellerWorker TestWorker()
        {
            var worker = new TellerWorker(new()
            {
                IPv4 = "localhost",
                Port = Constants.DefaultWorkerServerPort
            });

            return worker;
        }

        [Fact]
        public TellerWatcher TestWatcher()
        {
            var watcher = new TellerWatcher();

            return watcher;
        }

        [Fact]
        public void TestWorkerWatcherConnection()
        {
            var worker = new TellerWorker(new()
            {
                IPv4 = "localhost",
                Port = 51002
            });

            worker.Listen();

            var watcher = new TellerWatcher();
            watcher.Connect(worker);

            var directory = @"C:\Users\TechMech\Desktop";
            // var tree = watcher.RPCClient?.ListWorkerDirectory(new Directory { Directory_ = directory });

            // Assert.True(tree?.Tree.Count > 0);
        }
    }
}
