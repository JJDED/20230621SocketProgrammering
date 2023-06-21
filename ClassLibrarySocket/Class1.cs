using System.Net.Sockets;
using System.Text;

namespace ClassLibrarySocket;

public class SocketLibrary
{
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
}