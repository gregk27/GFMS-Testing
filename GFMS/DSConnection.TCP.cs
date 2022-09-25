using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public partial class DSConnection : IDisposable
    {
        private TcpClient _tcpClient;
        private NetworkStream _tcpStream;

        /// <summary>
        /// Called by constructor to setup TCP communications
        /// </summary>
        private void TCPInit(TcpClient client)
        {
            // Copy over TCP information
            _tcpClient = client;
            _tcpStream = client.GetStream();

            // Incoming TCP task
            Task.Run(async () =>
            {
                while (true)
                {
                    // Check for cancellation
                    if (_threadCancellation.IsCancellationRequested)
                        break;
                    // TODO: Implement processing for incoming TCP messages
                }
            }, _threadCancellation.Token);


            // Immediately send drive station info message
            StationInfoMessage smsg = new();
            smsg.Station = Station;
            smsg.Status = StationInfoMessage.StatusTypes.GOOD;
            var (data, len) = smsg.ToByteArray();
            _tcpStream.Write(data, 0, len);
        }

    }
}
