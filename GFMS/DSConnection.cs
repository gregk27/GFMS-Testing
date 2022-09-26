using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    internal partial class DSConnection : IDisposable
    {
        public readonly IPAddress IPAddress;

        private CancellationTokenSource _threadCancellation;
        private Func<FMStoDS> _stateProvider;

        public DSConnection(Func<FMStoDS> stateProvider, TcpClient client, IPAddress destination)
        {
            IPAddress = destination;
            _stateProvider = stateProvider;

            _threadCancellation = new();

            UDPInit();

            TCPInit(client);
        }

        ~DSConnection()
        {
            Dispose();
        }

        public void Dispose()
        {
            // End sending thread when done
            _threadCancellation.Cancel();
        }
    }
}
