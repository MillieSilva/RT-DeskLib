// System Imports

// Library Imports
using Library.Network.Teller;

// External Imports
using Grpc.Core;
using Grpc.Health.V1;
using Grpc.HealthCheck;


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
            var health = new HealthServiceImpl();
            
            health.SetStatus("", HealthCheckResponse.Types.ServingStatus.Serving);
            health.SetStatus(nameof(Authentication), HealthCheckResponse.Types.ServingStatus.Serving);
            health.SetStatus(nameof(FileTeller), HealthCheckResponse.Types.ServingStatus.Serving);

            WorkerServer = new Server
            {
                Services = {
                    Health.BindService(health),
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
