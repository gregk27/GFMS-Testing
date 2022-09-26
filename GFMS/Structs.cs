namespace GFMS
{
    public readonly struct Station
    {
        public readonly byte Number;
        public readonly Alliance Alliance;

        public Station(Alliance alliance, byte stationNum)
        {
            Alliance = alliance;
            Number = stationNum;
        }

        public override string ToString() => $"{Alliance} {Number}";

        // Predefined standard stations
        public static Station RED_1 => new(Alliance.RED, 1);
        public static Station RED_2 => new(Alliance.RED, 2);
        public static Station RED_3 => new(Alliance.RED, 3);
        public static Station BLUE_1 => new(Alliance.BLUE, 1);
        public static Station BLUE_2 => new(Alliance.BLUE, 2);
        public static Station BLUE_3 => new(Alliance.BLUE, 3);
    }

    public readonly struct Match
    {
        public readonly TournamentLevel Level;
        public readonly ushort Number;
        public readonly byte Replay;

        public Match(TournamentLevel level, ushort number, byte replay = 0)
        {
            Level = level;
            Number = number;
            Replay = replay;
        }

        public override string ToString()
        {
            if (Replay == 0)
                return $"{Level} {Number}";
            else
                return $"{Level} {Number} Replay {Replay}";
        }
    }

    public readonly struct MatchConfig
    {

        public readonly Match Number;
        private readonly Dictionary<ushort, DriveStation> StationMappings = new();
        internal readonly DriveStation[] Stations;

        public MatchConfig(Match number, DriveStation[] stations)
        {
            Number = number;
            Stations = stations;
            foreach (var station in stations)
            {
                StationMappings.Add(station.TeamNumber, station);
                station.SetMatchInfo(number);
            }
        }

        internal DriveStation? GetTeamStation(ushort teamNum)
        {
            if (StationMappings.ContainsKey(teamNum))
                return StationMappings[teamNum];
            return null;
        }

    }
}
