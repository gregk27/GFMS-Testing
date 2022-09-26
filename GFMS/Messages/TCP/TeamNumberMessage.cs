namespace GFMS.Messages.TCP
{
    internal class TeamNumberMessage : TagMessage
    {
        public ushort TeamNumber
        {
            get
            {
                int idx = 0;
                return ReadShort(Payload, ref idx);
            }
            set
            {
                byte[] tmp = Payload;
                int idx = 0;
                WriteShort(value, ref tmp, ref idx);
                Payload = tmp;
            }
        }

        public TeamNumberMessage() : base(TagTypes.TEAM_NUMBER, new byte[2])
        {

        }

        public TeamNumberMessage(TagMessage tm) : base(tm)
        {
            if (Type != TagTypes.TEAM_NUMBER)
                throw new Exception("Invalid tag type");

        }
    }
}
