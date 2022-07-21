// System Imports

// Library Imports
using Library.Network;
using Library.Network.Teller;

// External Imports
using Xunit;


namespace Tests;

public class Teller
{
    [Fact]
    public TellerWorker TestWorker()
    {
        var worker = new TellerWorker(new ConnectionInfo
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
    public bool TestWorkerWatcherConnection()
    {
        var worker = new TellerWorker(new ConnectionInfo
        {
            IPv4 = "localhost",
            Port = Constants.DefaultWorkerServerPort
        });

        worker.Listen();

        var watcher = new TellerWatcher();
        watcher.Connect(worker);

        var rpc = watcher.GetWorkerRPC(worker);

        return rpc != null && rpc.CheckServices();
    }
}
