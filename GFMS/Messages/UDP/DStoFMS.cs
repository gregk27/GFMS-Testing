namespace GFMS.Messages.UDP
{
    internal class DStoFMS : Message
    {
        public ushort SequenceNum;

        private byte _statusByte;
        public bool EStopped
        {
            get => CheckBit(_statusByte, 7);
            set => SetBit(ref _statusByte, 7, value);
        }
        public bool CommsActive
        {
            get => CheckBit(_statusByte, 5);
            set => SetBit(ref _statusByte, 5, value);
        }
        public bool RadioConnected
        {
            get => CheckBit(_statusByte, 4);
            set => SetBit(ref _statusByte, 4, value);
        }
        public bool RioConnected
        {
            get => CheckBit(_statusByte, 3);
            set => SetBit(ref _statusByte, 3, value);
        }
        public bool Enabled
        {
            get => CheckBit(_statusByte, 2);
            set => SetBit(ref _statusByte, 2, value);
        }
        public Mode Mode
        {
            get => (Mode)(_statusByte & 0b00000011);
            set
            {
                // Clear bits for value
                _statusByte &= 0b11111100;
                // Or in value
                _statusByte |= (byte)value;
            }
        }

        /// <summary>
        /// Raw byte representation of battery voltage
        /// Per docs actual voltage is calculated from byte XXYY as XX + YY/256
        /// 
        /// Rounded to 2 decimal places as that's limit of the accuracy supported by the format (+/- 0.004)
        /// </summary>
        private ushort _voltageRaw;
        public double BatteryVoltage
        {
            get => Math.Round((_voltageRaw >> 8) + (_voltageRaw & 0xFF) / 256.0, 2);
            set => _voltageRaw = (ushort)((byte)value << 8 | (byte)(value % 1 * 256));
        }

        public ushort TeamNum;

        public override (byte[], int) ToByteArray()
        {
            byte[] data = new byte[64];
            int idx = 0;

            // Sequence Num
            WriteShort(SequenceNum, ref data, ref idx);
            // Comm Version
            data[idx++] = 0;
            // Control byte
            data[idx++] = _statusByte;

            // Team Number
            WriteShort(TeamNum, ref data, ref idx);

            // Battery
            WriteShort(_voltageRaw, ref data, ref idx);

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
            _statusByte = data[idx++];

            // Team Number
            TeamNum = ReadShort(data, ref idx);

            // Battery
            _voltageRaw = ReadShort(data, ref idx);
        }
    }
}
