// System Usings

// Library Usings

// External Usings
using Grpc.Core;


namespace Library.Network.RPC.Worker
{
    internal class WorkerAuthenticationRPC : Authentication.AuthenticationBase
    {
        public WorkerAuthenticationRPC(WorkerRPC workerRPC) {}

        public override Task<AuthenticationResponse> Authenticate(Credentials request, ServerCallContext context)
        {
            var response = new AuthenticationResponse() { Response = Response.Ok };

            return Task.FromResult(response);
        }
    }
}
