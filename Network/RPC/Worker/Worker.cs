// System Imports

// Library Imports
using Library.Network.Teller;

// External Imports
using Grpc.Core;


namespace Library.Network.RPC.Worker
{
    public class WorkerRPC
    {
        public Server? WorkerServer { get; private set; }
        TellerWorker Worker { get; init; }

        public WorkerRPC(TellerWorker worker)
        {
            Worker = worker;
        }

        public void Listen()
        {
            WorkerServer = new Server()
            {
                Services = {
                    Authentication.BindService(new WorkerAuthenticationRPC(this)),
                    FileTeller.BindService(new WorkerTellerRPC(this)),
                },
                Ports = { new ServerPort(Worker.ConnectionInfo.IPv4, Worker.ConnectionInfo.Port, ServerCredentials.Insecure), },
            };

            WorkerServer.Start();
        }

        public void Deafen()
        {
            WorkerServer?.ShutdownAsync().Wait();
        }
    }

}
