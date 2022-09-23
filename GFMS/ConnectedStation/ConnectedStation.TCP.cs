using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public class ConnectedStation : IDisposable
    {
        private const Mode DEFAULT_MODE = Mode.TELE;
        private const int DS_PORT = 1121;

        public DStoFMS LastRecv { get; private set; }
        private FMStoDS _lastSent;

        public readonly IPAddress IPAddress;
        public readonly ushort TeamNumber;
        public readonly Station Station;

        private TcpClient _tcpClient;
        private NetworkStream _tcpStream;
        private UdpClient _sock;
        private CancellationTokenSource _sendingThread;

        public ConnectedStation(ushort teamNumber, Station station, TcpClient client, IPAddress destination)
        {
            IPAddress = destination;
            TeamNumber = teamNumber;
            Station = station;

            // Initialize state information
            _lastSent = new FMStoDS();
            _lastSent.Station = station;
            _lastSent.Mode = DEFAULT_MODE;

            // Copy over TCP information
            _tcpClient = client;
            _tcpStream = client.GetStream();

            // Establish UDP connection
            _sock = new();
            _sock.Connect(new IPEndPoint(IPAddress, DS_PORT));

            _sendingThread = new();

            // UDP heartbeat task
            Task.Run(async () =>
            {
                while (true)
                {
                    // Check for cancellation
                    if (_sendingThread.IsCancellationRequested)
                        break;
                    // Send message
                    SendMessage();
                    // Wait between messages (should be ~500ms between messages)
                    await Task.Delay(450);
                }
            }, _sendingThread.Token);

            // Incoming TCP task
            Task.Run(async () =>
            {
                while (true)
                {
                    // Check for cancellation
                    if (_sendingThread.IsCancellationRequested)
                        break;
                    // TODO: Implement processing for incoming TCP messages
                }
            }, _sendingThread.Token);

            // Immediately send drive station info message
            StationInfoMessage smsg = new();
            smsg.Station = station;
            smsg.Status = StationInfoMessage.StatusTypes.GOOD;
            var (data, len) = smsg.ToByteArray();
            _tcpStream.Write(data, 0, len);
        }

        ~ConnectedStation()
        {
            Dispose();
        }

        public void Dispose()
        {
            // End sending thread when done
            _sendingThread.Cancel();
        }

        private void SendMessage()
        {
            byte[] data;
            int length;
            lock (_lastSent)
            {
                _lastSent.SequenceNum++;
                (data, length) = _lastSent.ToByteArray();
            }
            _sock.Send(data, length);
        }

        public void RecvMessage(DStoFMS message)
        {
            if (LastRecv == null || message.SequenceNum > LastRecv.SequenceNum)
                LastRecv = message;
        }

        /// <summary>
        /// Periodic update function to be called during match play
        /// Sets mode and remaining time
        /// </summary>
        /// <param name="mode">Current match mode</param>
        /// <param name="remainingTime">Time left in mode</param>
        public void MatchPeriodic(Mode mode, ushort remainingTime)
        {
            lock (_lastSent)
            {
                _lastSent.RemainingTime = remainingTime;
                _lastSent.Mode = mode;
            }
        }

        public void SetMatchData(Match match)
        {
            _lastSent.Match = match;
        }

        public void SetEnabled(bool enabled)
        {
            lock (_lastSent)
            {
                _lastSent.Enabled = enabled;
                // Send enable state change as priority message
                SendMessage();
            }
        }

        public void EStop()
        {
            lock (_lastSent)
            {
                _lastSent.EStopped = true;
                // Send E-Stop as priority message
                SendMessage();
            }
        }
    }
}
