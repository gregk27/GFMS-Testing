using GFMS.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GFMS
{
    public class Director
    {
        public static Dictionary<IPAddress, ConnectedStation> Stations = new();
        public static Dictionary<int, Station> StationMappings = new()
        {
            {1, Station.RED_1 },
            {2, Station.RED_2 },
            {3, Station.RED_3 },
            {4, Station.BLUE_1 },
            {5, Station.BLUE_1 },
            {6, Station.BLUE_1 },
        };

        public static Match CurrentMatch = new(TournamentLevel.TEST, 2);

        public void Setup()
        {
            byte[] data = new byte[1024];
            UdpClient newsock = new UdpClient(new IPEndPoint(IPAddress.Any, 1160));

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            Task.Run(() =>
            {
                data = newsock.Receive(ref sender);
                try
                {
                    var message = Message.FromBytes<DStoFMS>(data);
                    // If the sender is known, update last recevied message
                    if (Stations.ContainsKey(sender.Address))
                    {
                        lock (Stations)
                        {
                            Stations[sender.Address].UpdateRecv(message);
                        }
                    }
                    // If the sender is unkown but robot number is expected, add the new station
                    else if (StationMappings.ContainsKey(message.TeamNum))
                    {
                        var cs = new ConnectedStation(message, sender.Address, StationMappings[message.TeamNum]);
                        cs.SetMatchData(CurrentMatch);
                        cs.MatchPeriodic(Mode.AUTO, 20);
                        lock (Stations)
                        {
                            Stations.Add(sender.Address, cs);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"Message {data} from ${sender} could not be processed");
                }
            });
        }

        public void ClearStations()
        {
            lock (Stations)
            {
                foreach(var station in Stations.Keys)
                {
                    var s = Stations[station];
                    Stations.Remove(station);
                    s.Dispose();
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            lock (Stations)
            {
                foreach(var station in Stations)
                {
                    station.Value.SetEnabled(enabled);
                }
            }
        }
    }
}
