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
            get => CheckBit(_controlByte, 7);
            set => SetBit(ref _controlByte, 7, value);
        }
        public bool Enabled
        {
            get => CheckBit(_controlByte, 2);
            set => SetBit(ref _controlByte, 2, value);
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
            WriteShort(SequenceNum, ref data, ref idx);
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
            WriteInt(((uint)Timestamp.Millisecond)*1000, ref data, ref idx);
            data[idx++] = (byte)Timestamp.Second;
            data[idx++] = (byte)Timestamp.Minute;
            data[idx++] = (byte)Timestamp.Hour;
            data[idx++] = (byte)Timestamp.Day;
            data[idx++] = (byte)Timestamp.Month;
            data[idx++] = (byte)(Timestamp.Year-1900);

            // Remaining time in mode
            WriteShort(RemainingTime, ref data, ref idx);

            return (data, idx);
        }

        protected override void FromByteArray(byte[] data)
        {
            int idx = 0;

            // Sequence Num
            SequenceNum = ReadShort(data, ref idx);
            // Comm Version
            _ = data[idx++];
            // Control byte
            _controlByte = data[idx++];
            // Request
            _ = data[idx++];

            // Alliance station
            _stationInfo = data[idx++];
            // Tournament Level (Test)
            Level = (TournamentLevel)data[idx++];
            // Match
            MatchNum = data[idx++];
            // Play/Replay
            ReplayCount = data[idx++];

            // Date
            var microsecond = ReadInt(data, ref idx);
            var second = data[idx++];
            var minute = data[idx++];
            var hour = data[idx++];
            var day = data[idx++];
            var month = data[idx++];
            var year = data[idx++]+1900;
            Timestamp = new DateTime(year, month, day, hour, minute, second, (int)(microsecond/1000));

            // Remaining time in mode
            RemainingTime = ReadShort(data, ref idx);
        }
    }
}
