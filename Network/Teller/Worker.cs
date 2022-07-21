// System Imports

// Library Imports
using Library.Network.RPC.Worker;
using Library.Network.Broker;

// External Imports


namespace Library.Network.Teller
{
    public class TellerWorker
    {
        public ConnectionInfo ConnectionInfo { get; }
        internal BrokerWorkerRestClient BrokerClient;
        public WorkerRPC RPC { get; }

        public Guid UUID;
        public bool Authenticated => UUID != default;

        public TellerWorker(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;

            BrokerClient = new BrokerWorkerRestClient();
            RPC = new WorkerRPC(this);
        }

        public void Listen()
        {
            RPC.Listen();
        }

        public void Deafen()
        {
            RPC.Deafen();
        }

        //

        public async void Authenticate(string hwid)
        {
            UUID = await BrokerClient.Synchronize(hwid);
        }

    }

}
