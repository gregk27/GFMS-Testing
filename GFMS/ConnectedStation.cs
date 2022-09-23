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

        public event EventHandler<ConnectedStation> OnStateChanged;

        /// <summary>
        /// FMS to DS message, also used to hold robot's current state
        /// </summary>
        private FMStoDS _state;

        private CancellationTokenSource _threadCancellation;

        public ConnectedStation(ushort teamNumber, Station station, TcpClient client, IPAddress destination)
        {
            IPAddress = destination;
            TeamNumber = teamNumber;
            Station = station;

            // Initialize state information
            _state = new FMStoDS();
            _state.Station = station;
            _state.Mode = DEFAULT_MODE;

            _threadCancellation = new();

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
            _threadCancellation.Cancel();
        }

        //----------------------
        // FMS state properties
        //----------------------

        /// <summary>
        /// Periodic update function to be called during match play
        /// Sets mode and remaining time
        /// </summary>
        /// <param name="mode">Current match mode</param>
        /// <param name="remainingTime">Time left in mode</param>
        public void MatchPeriodic(Mode mode, ushort remainingTime)
        {
            lock (_state)
            {
                _state.RemainingTime = remainingTime;
                _state.Mode = mode;
            }
        }

        public void SetMatchData(Match match)
        {
            _state.Match = match;
        }

        public void SetEnabled(bool enabled)
        {
            lock (_state)
            {
                _state.Enabled = enabled;
                // Send enable state change as priority message
                SendMessage();
            }
        }

        public void EStop()
        {
            lock (_state)
            {
                _state.EStopped = true;
                // Send E-Stop as priority message
                SendMessage();
            }
        }


        //------------------------
        // DS reported properties
        //------------------------
        public bool HasComms => _lastRecv?.CommsActive ?? false;

        public bool IsEnabled => _lastRecv?.Enabled ?? false;

        public bool IsEstopped => _lastRecv?.EStopped ?? false;

        public double BatteryVoltage => _lastRecv?.BatteryVoltage ?? 0;
    }
}
