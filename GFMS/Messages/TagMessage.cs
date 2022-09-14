namespace GFMS.Messages
{
    public class TagMessage : Message
    {

        public byte ID;
        public byte[] Payload { get; set; }

        public override (byte[], int) ToByteArray()
        {
            int idx = 0;
            byte[] data = new byte[Payload.Length + 3];
            
            // Set length header
            WriteShort((ushort)(Payload.Length + 1), ref data, ref idx);
            data[idx++] = ID;
            Payload.CopyTo(data, idx);
            return (data, data.Length);
        }

        protected override void FromByteArray(byte[] data)
        {
            int idx = 0;
            ushort size = ReadShort(data, ref idx);
            ID = data[idx++];
            Payload = data[idx..size];
        }

    }

    public enum TagTypes
    {
        WPILIB_VERSION = 0x00,
        RIO_VERSION = 0x01,
        DS_VERSION = 0x02,
        PDP_VERSION = 0x03,
        PCM_VERSION = 0x04,
        CANJAG_VERSION = 0x05,
        CANTALON_VERSION = 0x06,
        THIRD_PARTY_DEVICE_VERSION = 0x07,
        
        EVENT_CODE = 0x14,
        USAGE_REPORT = 0x15,
        LOG_DATA = 0x16,
        ERR0R_AND_EVENT_DATA = 0x17,
        TEAM_NUMBER = 0x18,
        STATION_INFO = 0x19,
        CHALLENGE_QUESTION = 0x1A,
        CHALLENGE_RESPONSE = 0x1B,
        GAME_DATA = 0x1C,
        DS_PING = 0x1D,
    }
}
