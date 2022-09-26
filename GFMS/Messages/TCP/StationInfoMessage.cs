namespace GFMS.Messages.TCP
{
    internal class StationInfoMessage : TagMessage
    {
        // Taken from FMS to DS message
        public byte StationNum
        {
            get => (byte)(Payload[0] % 3 + 1);
            set
            {
                if (Alliance == Alliance.RED)
                    Payload[0] = (byte)(value - 1);
                else
                    Payload[0] = (byte)(value + 2);
            }
        }
        public Alliance Alliance
        {
            get => Payload[0] < 3 ? Alliance.RED : Alliance.BLUE;
            set
            {
                if (value == Alliance.RED)
                    Payload[0] = (byte)(StationNum - 1);
                else
                    Payload[0] = (byte)(StationNum + 2);
            }
        }
        public Station Station
        {
            get => new Station(Alliance, StationNum);
            set
            {
                StationNum = value.Number;
                Alliance = value.Alliance;
            }
        }

        public StatusTypes Status
        {
            get => (StatusTypes)Payload[1];
            set => Payload[1] = (byte)value;
        }

        public StationInfoMessage() : base(TagTypes.STATION_INFO, new byte[2]) { }

        public StationInfoMessage(TagMessage tm) : base(tm)
        {
            if (Type != TagTypes.STATION_INFO)
                throw new Exception("Invalid tag type");
        }

        public enum StatusTypes
        {
            GOOD = 0,
            BAD = 1,
            WAITING = 2,
        }
    }
}
