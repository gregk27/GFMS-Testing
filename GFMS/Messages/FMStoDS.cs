namespace GFMS.Messages
{
    public class FMStoDS : Message
    {
        // Sequence number
        public ushort SequenceNum;

        // Control Byte
        private byte _controlByte;
        public bool EStopped
        {
            get => (_controlByte & 0b10000000) != 0;
            set
            {
                if (value)
                    _controlByte |= 0b10000000;
                else
                    _controlByte &= 0b01111111;
            }
        }
        public bool Enabled
        {
            get => (_controlByte & 0b00000100) != 0;
            set
            {
                if (value)
                    _controlByte |= 0b00000100;
                else
                    _controlByte &= 0b11111011;
            }
        }
        public Mode Mode
        {
            get => (Mode)(_controlByte & 0b00000011);
            set
            {
                // Clear bits for value
                _controlByte &= 0b11111100;
                // Or in value
                _controlByte |= (byte)value;
            }
        }

        private byte _stationInfo;
        public byte StationNum
        {
            get => (byte)(_stationInfo % 3 + 1);
            set {
                if (Alliance == Alliance.RED)
                    _stationInfo = (byte)(value - 1);
                else
                    _stationInfo = (byte)(value + 2);
            }
        }
        public Alliance Alliance
        {
            get => _stationInfo < 3 ? Alliance.RED : Alliance.BLUE;
            set {
                if (value == Alliance.RED)
                    _stationInfo = (byte)(StationNum - 1);
                else
                    _stationInfo = (byte)(StationNum + 2);
            }
        }

        public TournamentLevel Level;
        public byte MatchNum;
        public byte ReplayCount;

        // Remaining time in mode
        public ushort RemainingTime;

        /// <summary>
        /// Timestamp of creation or last call to ToByteArray(), whichever is more recent
        /// </summary>
        public DateTime Timestamp = DateTime.Now;

        public override (byte[], int) ToByteArray()
        {
            byte[] data = new byte[64];
            int idx = 0;

            // Sequence Num
            WriteNumber(SequenceNum, ref data, ref idx);
            // Comm Version
            data[idx++] = 0;
            // Control byte
            data[idx++] = _controlByte;
            // Request
            data[idx++] = 0;

            // Alliance station
            data[idx++] = _stationInfo;
            // Tournament Level (Test)
            data[idx++] = (byte)Level;
            // Match
            data[idx++] = MatchNum;
            // Play/Replay
            data[idx++] = ReplayCount;

            // Date
            Timestamp = DateTime.Now;
            uint miliseconds = (uint)((DateTimeOffset)Timestamp).ToUnixTimeSeconds();
            WriteNumber(miliseconds, ref data, ref idx);
            data[idx++] = (byte)Timestamp.Second;
            data[idx++] = (byte)Timestamp.Minute;
            data[idx++] = (byte)Timestamp.Hour;
            data[idx++] = (byte)Timestamp.Day;
            data[idx++] = (byte)Timestamp.Month;
            data[idx++] = (byte)Timestamp.Year;

            // Remaining time in mode
            WriteNumber(RemainingTime, ref data, ref idx);

            return (data, idx);
        }
    }
}
