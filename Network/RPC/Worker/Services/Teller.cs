// System Usings

// Library Usings

// External Usings
using Google.Protobuf;
using Grpc.Core;


namespace Library.Network.RPC.Worker
{
    internal class WorkerTellerRPC : FileTeller.FileTellerBase
    {
        public WorkerTellerRPC(WorkerRPC workerRPC) {}

        public override async Task ListDirectory(Directory request, IServerStreamWriter<Path> responseStream, ServerCallContext context)
        {
            var tree = new List<string>();

            try
            {
                var directoryTree = System.IO.Directory.GetFileSystemEntries(request.Directory_, "*", SearchOption.TopDirectoryOnly);

                foreach (var path in directoryTree)
                {
                    var name = System.IO.Path.GetFileName(path);
                    tree.Add(name);
                }

            }
            catch (Exception)
            {
            }

            foreach (var path in tree)
                await responseStream.WriteAsync(new Path() { Path_ = path });
        }

        public override Task<FileMetadata>? RequestFileMetadata(Path request, ServerCallContext context)
        {
            var info = new FileInfo(request.Path_);

            if (info == null)
                return null;

            var metadata = new FileMetadata()
            {
                Name = info.Name,
                Size = info.Length
            };

            return Task.FromResult(metadata);
        }


        public override async Task RequestFile(Path request, IServerStreamWriter<File> responseStream, ServerCallContext context)
        {
            var info = new FileInfo(request.Path_);

            if (info == null)
                return;

            using var stream = System.IO.File.Open(info.FullName, FileMode.Open);
            {
                int bytesRead;
                var buffer = new byte[64 * 1024];

                while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
                {
                    await responseStream.WriteAsync(new File
                    {
                        Chunk = ByteString.CopyFrom(buffer[0..bytesRead]),
                    });
                }
            }
        }
    }
}
