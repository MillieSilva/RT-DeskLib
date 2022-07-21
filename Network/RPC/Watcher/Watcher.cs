// System Imports
using System.Collections.ObjectModel;

// Library Imports
using Library.Network.RPC.Watcher.Services;

// External Imports
using Grpc.Core;
using Grpc.Health.V1;
using Grpc.HealthCheck;


namespace Library.Network.RPC.Watcher
{
    public class WatcherRPC
    {
        internal Channel? Channel { get; init; }

        public ConnectionState State 
        {
            get 
            {
                switch (Channel?.State)
                {
                    case ChannelState.Idle:
                        return ConnectionState.Idle;
                        
                    case ChannelState.Connecting:
                        return ConnectionState.Checking;

                    case ChannelState.Ready:
                        return ConnectionState.Available;

                    case ChannelState.TransientFailure:
                        return ConnectionState.TrasientFailure;
                        
                    case ChannelState.Shutdown:
                        return ConnectionState.Offline;

                    default:
                        return ConnectionState.Unknown;
                }
            }
        }

        public WatcherAuthenticationRPC Authentication { get; }
        public WatcherTellerRPC Teller { get; }
        public Health.HealthClient Health { get; }
        
        public WatcherRPC(ConnectionInfo connectionInfo)
        {
            Channel = new(connectionInfo.IPv4, connectionInfo.Port, ChannelCredentials.Insecure);
            Channel.ConnectAsync();

            Authentication = new(Channel);
            Teller = new(Channel);
            Health = new Health.HealthClient(Channel);
        }

        public bool Authenticated { get; private set; }

        public async Task<bool> Connect()
        {
            var response = await Authentication.AuthenticateAsync(new Credentials { UUID = new Guid().ToString() });

            if (response.Response != Response.Ok)
                return false;

            return true;
        }

        public void Disconnect()
        {
            Channel?.ShutdownAsync().Wait();
        }

        //

        public string WorkingDirectory = @"C:\";
        public ObservableCollection<string> DirectoryTree;

        public async Task<string> ChangeDirectory(string directory)
        {
            WorkingDirectory = directory;

            DirectoryTree = await Teller.ListDirectoryAsync(directory);

            return WorkingDirectory;
        }

        public async Task TransferFile(string source, string target)
        {
            await Teller.RequestFile(source, target);
        }

        public async Task RequestFile(string source, string target)
        {
            await Teller.RequestFile(source, target);
        }

        public bool CheckServices()
        {
            var authenticationState = Health.Check(new HealthCheckRequest { Service = nameof(Authentication) });
            var tellerState = Health.Check(new HealthCheckRequest() { Service = nameof(FileTeller) });
            
            return authenticationState.Status == HealthCheckResponse.Types.ServingStatus.Serving
                && tellerState.Status == HealthCheckResponse.Types.ServingStatus.Serving;
        }
    }
}


public enum ConnectionState
{
    Checking,
    Idle,
    Available,
    Offline,
    TrasientFailure,
    Unknown
}
