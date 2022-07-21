// System Usings

// Library Usings

// External Usings

using Grpc.Core;

namespace Library.Network.RPC.Watcher.Services
{
    public class WatcherAuthenticationRPC : Authentication.AuthenticationClient
    {
        public WatcherAuthenticationRPC(Channel channel) : base(channel) {}
    }
}
