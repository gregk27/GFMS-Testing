using GFMS.Messages.UDP;

namespace GFMS
{
    public class DriveStation : IDisposable
    {

        public readonly ushort TeamNumber;
        public readonly Station Station;
        internal FMStoDS State { get; private set; }
        private DSConnection? _connection;

        public bool IsConnected => _connection != null;

        public event EventHandler<DriveStation>? OnConnect;
        public event EventHandler<DriveStation>? OnStateChanged;
        public event EventHandler<DriveStation>? OnDisconnect;

        public DriveStation(ushort teamNumber, Station station)
        {
            TeamNumber = teamNumber;
            Station = station;

            State = new();
            State.Station = station;
        }

        public void Dispose()
        {
            Disconnect();
        }

        //---------------------------------------------
        // Control functions reserved for internal use
        //---------------------------------------------

        internal void Connect(DSConnection _connetion)
        {
            if(_connection != null)
                _connection.Dispose();
            _connection = _connetion;
            OnConnect?.Invoke(this, this);
        }

        internal void Disconnect()
        {
            if (_connection != null)
                _connection.Dispose();
            _connection = null;
            OnDisconnect?.Invoke(this, this);
        }

        internal void SetMatchInfo(Match match)
        {
            State.Match = match;
        }

        internal void MatchPeriodic(Mode m, ushort time)
        {
            State.Mode = m;
            State.RemainingTime = time;
        }

        //--------------------------------------------
        // Control functions exposed for external use
        //--------------------------------------------

        public void SetEnabled(bool enabled)
        {
            // Allow for disabling whenever but only enabling when connected and not e-stopped
            if (!enabled || (enabled && IsConnected && !State.EStopped))
            {
                State.Enabled = enabled;
                _connection?.SendMessage();
                OnStateChanged?.Invoke(this, this);
            }
        }

        public void EStop()
        {
            State.Enabled = false;
            State.EStopped = true;
            _connection?.SendMessage();
            OnStateChanged?.Invoke(this, this);
        }

        //--------------------------
        // Properties sent to robot
        //--------------------------

        /// <summary>
        /// Enabled state sent by FMS
        /// </summary>
        public bool IsEnabled => State.Enabled;

        /// <summary>
        /// E-Stop state sent by FMS
        /// </summary>
        public bool IsEStopped => State.EStopped;

        //------------------------
        // DS reported properties
        //------------------------

        public bool RobotComms => _connection?.LastRecv?.CommsActive ?? false;

        /// <summary>
        /// DS Reporting of robot's enabled status
        /// </summary>
        public bool RobotEnabled => _connection?.LastRecv?.Enabled ?? false;

        /// <summary>
        /// DS Reporting of robot's e-stop status
        /// </summary>
        public bool RobotEStopped => _connection?.LastRecv?.EStopped ?? false;
        
        /// <summary>
        /// DS Reporting of robot's operating mode
        /// </summary>
        public Mode RobotMode => _connection?.LastRecv?.Mode ?? Mode.TEST;

        public double BatteryVoltage => _connection?.LastRecv?.BatteryVoltage ?? 0;
    }
}