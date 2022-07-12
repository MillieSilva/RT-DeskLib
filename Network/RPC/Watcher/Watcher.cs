// System Imports
using System.Collections.ObjectModel;

// Library Imports

// External Imports
using Grpc.Core;


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
                    case ChannelState.Connecting:
                        return ConnectionState.Checking;

                    case ChannelState.Ready:
                        return ConnectionState.Available;

                    case ChannelState.TransientFailure:
                    case ChannelState.Shutdown:
                        return ConnectionState.Offline;

                    default:
                        return ConnectionState.Offline;
                }
            }
        }

        public WatcherAuthenticationRPC Authentication { get; init; }
        public WatcherTellerRPC Teller { get; init; }

        public WatcherRPC(ConnectionInfo connectionInfo) : base()
        {
            Channel = new(connectionInfo.IPv4, connectionInfo.Port, ChannelCredentials.Insecure);
            Channel.ConnectAsync();

            Authentication = new(Channel);
            Teller = new(Channel);
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

        public void TransferFile(string source, string target)
        {
            Teller.RequestFile(source, target);
        }

        public void RequestFile(string source, string target)
        {
            Teller.RequestFile(source, target);
        }
    }
}


public enum ConnectionState
{
    Checking,
    Available,
    Offline
}
