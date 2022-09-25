using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public partial class DSConnection : IDisposable
    {
        private const int DS_PORT = 1121;
        private const int INCOMING_TIMEOUT = 1500;

        private DStoFMS? _lastRecv;
        public bool IsAlive { get; private set; }
        private long _lastRecvTime;

        private UdpClient _sock;


        public event EventHandler OnDisconnect;

        /// <summary>
        /// Called by constructor to setup UDP communications
        /// </summary>
        private void UDPInit()
        {
            // Establish UDP connection
            _sock = new();
            _sock.Connect(new IPEndPoint(IPAddress, DS_PORT));

            _lastRecvTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            IsAlive = true;

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
                    
                    // If no messages have arrived in the timeout period, then assume the connection is dead
                    if(DateTimeOffset.Now.ToUnixTimeMilliseconds() - _lastRecvTime > INCOMING_TIMEOUT)
                    {
                        IsAlive = false;
                        OnDisconnect.Invoke(this, EventArgs.Empty);
                    }

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
            if (_lastRecv == null || message.SequenceNum > _lastRecv.SequenceNum)
            {
                _lastRecv = message;
                _lastRecvTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

    }
}
