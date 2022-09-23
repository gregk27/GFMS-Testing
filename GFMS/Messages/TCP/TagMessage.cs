namespace GFMS.Messages.TCP
{
    public class TagMessage : Message
    {

        public virtual TagTypes Type { get; protected set; }
        public virtual byte[] Payload { get; set; }

        /// <summary>
        /// Constructor for Message.FromBytes, should not be used otherwise as Type cannot be set elsewhere
        /// </summary>
        public TagMessage() { }

        public TagMessage(TagTypes type, byte[] payload)
        {
            Type = type;
            Payload = payload;
        }

        /// <summary>
        /// Copy-constructor used by derived classes
        /// </summary>
        protected TagMessage(TagMessage tm)
        {
            Type = tm.Type;
            Payload = tm.Payload;
        }

        public override (byte[], int) ToByteArray()
        {
            int idx = 0;
            byte[] data = new byte[Payload.Length + 3];

            // Set length header
            WriteShort((ushort)(Payload.Length + 1), ref data, ref idx);
            data[idx++] = (byte)Type;
            Payload.CopyTo(data, idx);
            return (data, data.Length);
        }

        protected override void FromByteArray(byte[] data)
        {
            int idx = 0;
            ushort size = (ushort)(ReadShort(data, ref idx) + 2);
            Type = (TagTypes)data[idx++];
            Payload = data[idx..size];
        }

        /// <summary>
        /// Improved byte array processing that creates derived class if appropriate
        /// </summary>
        public static TagMessage FromBytes(byte[] data)
        {
            TagMessage msg = new TagMessage();
            msg.FromByteArray(data);

            switch (msg.Type)
            {
                case TagTypes.TEAM_NUMBER:
                    return new TeamNumberMessage(msg);
                default:
                    return msg;
            }
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
