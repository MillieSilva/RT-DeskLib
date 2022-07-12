// System Imports
using System.Collections.ObjectModel;

// Library Imports
using Library.Network.Broker;
using Library.Network.RPC.Watcher;

// External Imports


namespace Library.Network.Teller
{

    public class TellerWatcher
    {
        public BrokerWatcherRestClient BrokerClient = new();

        public ObservableCollection<Guid> WorkerIds = new();
        public ObservableCollection<WorkerMetadata> WorkersMetadata = new();
        public Dictionary<Guid, WatcherRPC> WorkersConnections = new();

        public string WorkingDirectory = @"C:\";
        public ObservableCollection<string> DirectoryTree = new();

        public TellerWatcher()
        {
            if (!BrokerClient.IsAvailable())
                return;

            WorkerIds = BrokerClient.RequestAllWorkerIds().Result;
        }

        public WorkerMetadata? FetchWorkerMetadata(Guid uuid)
        {
            var workerMetadataTask = BrokerClient.RequestWorkerMetadata(uuid);


            if (workerMetadataTask.Result.Equals(default(WorkerMetadata)))
                return null;

            var workerMetadata = workerMetadataTask.Result;

            WorkersMetadata.Add(workerMetadata);

            Connect(workerMetadata);

            return workerMetadata;
        }

        public WatcherRPC? GetWorkerRPC(WorkerMetadata workerMetadata)
        {
            if (!WorkersConnections.ContainsKey(workerMetadata.UUID))
                return null;

            return WorkersConnections[workerMetadata.UUID];
        }
        
        public void Connect(Guid uuid, ConnectionInfo connectionInfo)
        {
            if (!WorkersConnections.ContainsKey(uuid))
                WorkersConnections[uuid] = (new WatcherRPC(connectionInfo));
        }

        public void Connect(TellerWorker worker)
        {
            Connect(worker.UUID, worker.ConnectionInfo);
        }

        public void Connect(WorkerMetadata workerMetadata)
        {
            Connect(workerMetadata.UUID, new ConnectionInfo()
            {
                IPv4 = workerMetadata.Public_IPV4[0],
                Port = Constants.DefaultWorkerServerPort
            });;
        }

        public void ChangeDirectory(string directory)
        {
            DirectoryTree.Clear();

            try
            {
                var tree = System.IO.Directory.GetFileSystemEntries(directory, "*", SearchOption.TopDirectoryOnly);

                foreach (var node in tree)
                {
                    var name = System.IO.Path.GetFileName(node);
                    DirectoryTree.Add(name);
                }

            }
            catch (Exception)
            {
            }


            WorkingDirectory = directory;
        }
    }
}
