using System.Net;
using System.Net.Sockets;
using System.Text;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Task to send FMS data to DS
Task.Run(() =>
{
    byte[] data = new byte[1024];
    //IPEndPoint ipep = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 0, 2 }), 1120);
    UdpClient newsock = new UdpClient();

    Console.WriteLine("Waiting for a client...");


    IPEndPoint sender = new IPEndPoint(new IPAddress(new byte[] { 192, 168, 0, 2 }), 1120);

    Console.WriteLine($"Pending connection to {sender}");
    newsock.Connect(sender);
    Console.WriteLine("Connection established");
    //data = newsock.Receive(ref sender);

    //Console.WriteLine("Outgoing Message received from {0}:", sender.ToString());
    //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

    //string welcome = "Welcome to my test server";
    //data = Encoding.ASCII.GetBytes(welcome);
    //newsock.Send(data, data.Length, sender);

    UInt16 seq = 0;
    while (true)
    {
        //Console.WriteLine("Sending FMS Data");
        //Console.WriteLine("Heartbeat");
        //data = newsock.Receive(ref sender);

        //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
        int idx = 0;

        // Sequence Num
        var seqBytes = BitConverter.GetBytes(seq);
        data[idx++] = seqBytes[0];
        data[idx++] = seqBytes[1];
        // Comm Version
        data[idx++] = 0;
        // Control byte (Test Disabled)
        data[idx++] = 0b00000011;
        // Request
        data[idx++] = 0;

        // Alliance station (Red 2)
        data[idx++] = 1;
        // Tournament Level (Test)
        data[idx++] = 0;
        // Match
        data[idx++] = 1;
        // Play/Replay
        data[idx++] = 1;

        // Date
        UInt32 miliseconds = (UInt32)DateTimeOffset.Now.ToUnixTimeSeconds();
        var msBytes = BitConverter.GetBytes(miliseconds);
        data[idx++] = msBytes[0];
        data[idx++] = msBytes[1];
        data[idx++] = msBytes[2];
        data[idx++] = msBytes[3];
        data[idx++] = (byte)DateTime.Now.Second;
        data[idx++] = (byte)DateTime.Now.Minute;
        data[idx++] = (byte)DateTime.Now.Hour;
        data[idx++] = (byte)DateTime.Now.Day;
        data[idx++] = (byte)DateTime.Now.Month;
        data[idx++] = (byte)DateTime.Now.Year;

        // Remaining time in mode
        UInt16 remainingTime = 160;
        var rtBytes = BitConverter.GetBytes(remainingTime);
        data[idx++] = rtBytes[0];
        data[idx++] = rtBytes[1];

        idx += 4;
        //for(int i=0; i<idx; i++)
        //{
        //    Console.Write(data[i] + ",");
        //}
        //Console.WriteLine();

        newsock.Send(data, idx);

        seq++;
        Thread.Sleep(400);
    }

});

// Task to receive data from DS
Task.Run(() =>
{

    byte[] data = new byte[1024];
    IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 1160);
    UdpClient newsock = new UdpClient(ipep);

    Console.WriteLine("Waiting for a client...");

    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

    data = newsock.Receive(ref sender);

    Console.WriteLine("Connection from {0}:", sender.ToString());

    while (true)
    {
        data = newsock.Receive(ref sender);

        Console.WriteLine(string.Join(",", data));
        newsock.Send(data, data.Length, sender);
    }
});

// TCP Connection 
Task.Run(() =>
{
    TcpListener server = null;

    // Set the TcpListener on port 13000.
    Int32 port = 1750;
    IPAddress localAddr = new(new byte[] {192,168,0,2});

    // TcpListener server = new TcpListener(port);
    server = new TcpListener(localAddr, port);
        
    // Start listening for client requests.
    server.Start();


    // Perform a blocking call to accept requests.
    // You could also use server.AcceptSocket() here.
    TcpClient client = server.AcceptTcpClient();
    Console.WriteLine("TCP Connected!");

    Byte[] bytes = new Byte[256];

    // Get a stream object for reading and writing
    NetworkStream stream = client.GetStream();

    while (true)
    {
        String data = null;

        int i;
        // Loop to receive all the data sent by the client.
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            // Translate data bytes to a ASCII string.
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);

            // Process the data sent by the client.
            data = data.ToUpper();

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", data);
        }
    }
});

while (true)
{

}