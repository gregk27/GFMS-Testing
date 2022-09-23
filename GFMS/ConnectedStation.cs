using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
using System.Net;
using System.Net.Sockets;

namespace GFMS
{
    public partial class ConnectedStation : IDisposable
    {
        private const Mode DEFAULT_MODE = Mode.TELE;

        public readonly IPAddress IPAddress;
        public readonly ushort TeamNumber;
        public readonly Station Station;

        private CancellationTokenSource _sendingThread;

        public ConnectedStation(ushort teamNumber, Station station, TcpClient client, IPAddress destination)
        {
            IPAddress = destination;
            TeamNumber = teamNumber;
            Station = station;

            _sendingThread = new();

            UDPInit();

            TCPInit(client);
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
