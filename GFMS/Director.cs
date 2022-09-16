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
            {5, Station.BLUE_2 },
            {6, Station.BLUE_3 },
        };

        public static Match CurrentMatch = new(TournamentLevel.TEST, 2);

        public void Setup()
        {

            // Icomping UDP messages
            Task.Run(() =>
            {
                byte[] data = new byte[1024];
                UdpClient sock = new UdpClient(new IPEndPoint(IPAddress.Any, 1160));

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    data = sock.Receive(ref sender);
                    try
                    {
                        var message = Message.FromBytes<DStoFMS>(data);
                        // If the sender is known, update last recevied message
                        if (Stations.ContainsKey(sender.Address))
                        {
                            lock (Stations)
                            {
                                Stations[sender.Address].RecvMessage(message);
                            }
                        }
                        // If the sender is unkown but robot number is expected, add the new station
                        else if (StationMappings.ContainsKey(message.TeamNum))
                        {
                            Console.WriteLine($"Connecting team {message.TeamNum}@{sender.Address} to station {StationMappings[message.TeamNum]}");
                            var cs = new ConnectedStation(message, sender.Address, StationMappings[message.TeamNum]);
                            cs.SetMatchData(CurrentMatch);
                            cs.MatchPeriodic(Mode.AUTO, 20);
                            lock (Stations)
                            {
                                Stations.Add(sender.Address, cs);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Incoming message from {sender} dropped");
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Message [{string.Join(',', data)}] from {sender} could not be processed");
                    }
                }
            });


            // Broadcast to trigger DS connection
            Task.Run(() =>
            {
                byte[] data = new byte[64];
                UdpClient newsock = new();

                byte[] addr = new byte[] { 10, 100, 0, 0 };
                IPEndPoint reciever = new(new IPAddress(addr), 1121);

                while (true)
                {
                    // Loop over posssible IP addresses
                    addr[3]++;
                    reciever.Address = new IPAddress(addr);
                    // Don't send broadcast request to currently connected stations
                    if (Stations.ContainsKey(reciever.Address))
                        continue;
                    // Send empty message to trigger response
                    newsock.Client.SendTo(data, reciever);
                    // Slight delay between messages to reduce load
                    Thread.Sleep(100);
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
