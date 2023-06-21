using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _20230621SocketServer;

internal class SocketServer
{
    public SocketServer()
    {
        //new MultiThreading().StartThreading();
        StartServer();
    }
    void StartServer()
    {
        #region Endpoint Creation
        //Creates an endpoint by selecting the PC's hostname and getting the 
        //ipv4 network interfaces ip address list
        IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName(), AddressFamily.InterNetwork);
        int choice = ChooseIpAddress(iPHostEntry.AddressList);
        IPAddress iPAddress = iPHostEntry.AddressList[choice];
        //Or convert a string to type of IpAddress
        //IPAddress iPAddress2 = IPAddress.Parse("192.168.1.2");

        IPEndPoint iPEndPoint = new(iPAddress, 22222);
        #endregion
        //Creates socket that only listens and binds it with the server endpoint
        Socket listener = new(
            iPAddress.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        listener.Bind(iPEndPoint);
        listener.Listen(10);
        Console.WriteLine("Server listens on " + iPEndPoint);

        while (true)
        {
            //If a connection have been made with the listener socket,
            //a new thread is created that handles the traffic, therefore not
            //blocking 
            Socket handler = listener.Accept();
            Thread thread = new(new ThreadStart(() => ConnectToClient(handler)));
            thread.Start();
        }
    }

    /// <summary>
    /// Accepts connection from client, Recieves message 
    /// and returns message to client
    /// </summary>
    /// <param name="listener"></param>
    void ConnectToClient(Socket handler)
    {
        Console.WriteLine("Connect to: " + handler.RemoteEndPoint);
        while (true)
        {
            //This method will be a thread by itself, and keeps the 
            //connection open with the client socket, thus being
            //able to recieve messages without interuption.
            string? data = GetMessage(handler);
            byte[] returnMsg = Encoding.ASCII.GetBytes("Server received msg<EOM>");
            handler.Send(returnMsg);
            Console.WriteLine(data + $" ({handler.RemoteEndPoint})");
        }
    }

    /// <summary>
    /// Recieves and converts bytes from client message to ASCII,
    /// until End Of Message tag is recieved
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    string GetMessage(Socket socket)
    {
        string? data = null;
        byte[] bytes;

        while (true)
        {
            bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            if (data.Contains("<EOM>")) break;
        }
        return data;
    }

    /// <summary>
    /// Selects IP from ip address array
    /// </summary>
    /// <param name="addressList"></param>
    /// <returns>integer on array</returns>
    int ChooseIpAddress(IPAddress[] addressList)
    {
        int i = 0;
        foreach (var item in addressList)
            Console.WriteLine($"[{i++}] {item}");

        int j;
        do { Console.Write("Input number of Ipaddress: "); }
        while (!int.TryParse(Console.ReadLine(), out j)
            || j < 0
            || j >= addressList.Length);
        return j;
    }
}
