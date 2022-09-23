using GFMS.Messages;
using GFMS.Messages.TCP;
using GFMS.Messages.UDP;
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
        // Dummy main function to invoke director
        public static void Main()
        {
            Console.WriteLine("Hello World");
            new Director().Setup();
            while (true) ;
        }

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
            Task.Run(UDPListener);

            // TCP socket for tag comms
            Task.Run(TCPListener);
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

        /// <summary>
        /// Function invoked in task to listen and distribute incoming TCP messages
        /// </summary>
        private void UDPListener()
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
        }


        /// <summary>
        /// Function invoked in task to listen for incoming TCP connections
        /// </summary>
        private void TCPListener()
        {
            TcpListener server = null;

            // Set the TcpListener on port 13000.
            Int32 port = 1750;
            IPAddress localAddr = new(new byte[] { 10, 0, 100, 5 });

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(IPAddress.Any, port);

            // Start listening for client requests.
            server.Start();

            while (true)
            {
                // Perform a blocking call to accept requests.
                TcpClient client = server.AcceptTcpClient();

                // If the IP Address can't be obtained, disregard the message
                var ipep = client.Client.RemoteEndPoint as IPEndPoint;
                if (ipep == null) continue;

                // Just use a small byte array since it's only to process initial message
                var bytes = new byte[16];

                NetworkStream stream = client.GetStream();

                // Read first available message
                // The init message should safely fit within one call
                stream.Read(bytes, 0, bytes.Length);
                try
                {
                    // First incoming message should be the driver station's team number
                    TagMessage msg = TagMessage.FromBytes(bytes);
                    if (msg is TeamNumberMessage tmsg)
                    {
                        // If the team is expected to connect, proceede
                        if (StationMappings.ContainsKey(tmsg.TeamNumber))
                        {
                            if (Stations.ContainsKey(ipep.Address))
                            {
                                Console.WriteLine($"Re-Connection from {tmsg.TeamNumber}");
                                // Dispose of any remnants of existing connection
                                Stations[ipep.Address].Dispose();
                            }
                            else
                                Console.WriteLine($"Connecting team {tmsg.TeamNumber}@{ipep.Address} to station {StationMappings[tmsg.TeamNumber]}");

                            // Establish the new connection
                            var cs = new ConnectedStation(tmsg.TeamNumber, StationMappings[tmsg.TeamNumber], client, ipep.Address);
                            // Initialize match information
                            cs.SetMatchData(CurrentMatch);
                            cs.MatchPeriodic(Mode.AUTO, 20);
                            lock (Stations)
                            {
                                Stations.Add(ipep.Address, cs);
                            }
                            // Register disconnect callback
                            cs.OnDisconnect += (object? src, EventArgs e) =>
                            {
                                Console.WriteLine($"Team {cs.TeamNumber} disconnected unexpectedly");
                                cs.Dispose();
                                lock (Stations)
                                {
                                    Stations.Remove(ipep.Address);
                                }
                            };
                        }
                        else
                        {
                            Console.WriteLine($"Unexpected connection from {tmsg.TeamNumber}");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"Init TCP message from {ipep?.Address} could not be read. Message: {string.Join(",", bytes)}");
                }
            }
        }
    }
}
