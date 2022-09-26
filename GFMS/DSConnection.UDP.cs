using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public partial class DSConnection : IDisposable
    {
        private const int DS_PORT = 1121;
        private const int INCOMING_TIMEOUT = 1500;

        public DStoFMS? LastRecv;
        public bool IsAlive { get; private set; }
        private long _lastRecvTime;

        private UdpClient _sock;
        private ushort _seqNum = 0;

        public event EventHandler<DStoFMS>? OnMessageReceived;
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
            _seqNum = 0;

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

        /// <summary>
        /// Automatically invoked at ~500ms period
        /// Can be called externally for priority messages (such as enable/disable/e-stop)
        /// </summary>
        public void SendMessage()
        {
            byte[] data;
            int length;
            var toSend = _stateProvider();

            toSend.SequenceNum = _seqNum;
            (data, length) = toSend.ToByteArray();
            
            _sock.Send(data, length);
        }

        public void RecvMessage(DStoFMS message)
        {
            if (LastRecv == null || message.SequenceNum > LastRecv.SequenceNum)
            {
                LastRecv = message;
                _lastRecvTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                OnMessageReceived?.Invoke(this, message);
            }
        }

    }
}
