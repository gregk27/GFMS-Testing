using GFMS.Messages;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public class ConnectedStation : IDisposable
    {
        private const Mode DEFAULT_MODE = Mode.TELE;
        private const int DS_PORT = 1120;

        public DStoFMS LastRecv { get; private set; }
        private FMStoDS _lastSent;

        public readonly IPAddress IPAddress;
        public readonly ushort TeamNumber;
        public readonly byte StationNumber;
        public readonly Alliance Alliance;

        private UdpClient _sock;
        private CancellationTokenSource _sendingThread;

        public ConnectedStation(DStoFMS init, IPAddress destination, Alliance alliance, byte stationNumber)        {
            LastRecv = init;
            IPAddress = destination;
            TeamNumber = init.TeamNum;
            Alliance = alliance;
            StationNumber = stationNumber;

            // Initialize state information
            _lastSent = new FMStoDS();
            _lastSent.StationNum = stationNumber;
            _lastSent.Alliance = alliance;
            _lastSent.Mode = DEFAULT_MODE;

            // Establish UDP connection
            _sock = new();
            _sock.Connect(new IPEndPoint(IPAddress, DS_PORT));

            _sendingThread = new();

            Task.Run(async () =>
            {
                // Check for cancellation
                if (_sendingThread.IsCancellationRequested)
                    return;
                // Send message
                SendMessage();
                // Wait between messages (should be ~500ms between messages)
                await Task.Delay(450);
            }, _sendingThread.Token);
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

        public void SetMatchData(TournamentLevel level, byte matchNum, byte replayNum)
        {
            _lastSent.Level = level;
            _lastSent.MatchNum = matchNum;
            _lastSent.ReplayCount = replayNum;
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
