// System Usings
using System.Collections.ObjectModel;

// Library Usings

// External Usings
using Grpc.Core;


namespace Library.Network.RPC.Watcher
{
    public class WatcherTellerRPC : FileTeller.FileTellerClient
    {
        public WatcherTellerRPC(Channel channel) : base(channel) {}

        public async Task<ObservableCollection<string>> ListDirectoryAsync(string directory)
        {
            var tree = new ObservableCollection<string>();

            using (var call = ListDirectory(new Directory { Directory_ = directory }))
                while (await call.ResponseStream.MoveNext())
                    tree.Add(call.ResponseStream.Current.Path_);

            return tree;
        }

        public async Task RequestFile(string source, string target)
        {
            var metadata = await RequestFileMetadataAsync(new Path { Path_ = source });

            try
            {
                using var stream = System.IO.File.OpenWrite(target);

                using var call = RequestFile(new Path { Path_ = source });

                while (await call.ResponseStream.MoveNext())
                {
                    var chunk = call.ResponseStream.Current;

                    // TODO: Trusting that gRPC sends the bytes in proper order and quantity, if something
                    //       happens here, this can be looked at

                    stream.Write(chunk.Chunk.ToByteArray());
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
