// System Imports

// Library Imports

// External Imports
using Library.Network.Broker;


namespace Library.Network;

public class Constants
{
    public const ushort DefaultWorkerServerPort = 51001;

    public static string? GetBrokerAddress()
    {
#if DEBUG
        return "http://127.0.0.1:5000";
#else
        return Metadata.GetValidBrokerAddress()?.ToString();
#endif
    }

    public static string? ResolveWorkerServerAddress()
    {
#if DEBUG
        return "127.0.0.1";
#else
        return "0.0.0.0";
#endif
    }
}
