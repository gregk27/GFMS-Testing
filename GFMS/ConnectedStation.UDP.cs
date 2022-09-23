using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public partial class ConnectedStation : IDisposable
    {
        private const int DS_PORT = 1121;

        public DStoFMS LastRecv { get; private set; }

        private UdpClient _sock;


        /// <summary>
        /// Called by constructor to setup UDP communications
        /// </summary>
        private void UDPInit()
        {
            // Establish UDP connection
            _sock = new();
            _sock.Connect(new IPEndPoint(IPAddress, DS_PORT));

            // UDP heartbeat task
            Task.Run(async () =>
            {
                while (true)
                {
                    // Check for cancellation
                    if (_threadCancellation.IsCancellationRequested)
                        break;
                    // Send message
                    SendMessage();
                    // Wait between messages (should be ~500ms between messages)
                    await Task.Delay(450);
                }
            }, _threadCancellation.Token);
        }

        private void SendMessage()
        {
            byte[] data;
            int length;
            lock (_state)
            {
                _state.SequenceNum++;
                (data, length) = _state.ToByteArray();
            }
            _sock.Send(data, length);
        }

        public void RecvMessage(DStoFMS message)
        {
            if (LastRecv == null || message.SequenceNum > LastRecv.SequenceNum)
                LastRecv = message;
        }

    }
}
