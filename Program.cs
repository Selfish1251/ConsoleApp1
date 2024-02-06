using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class UDPChatClient
{
    private const int ServerPort = 8888;
    private const int ClientPort = 8889;
    private static UdpClient udpClient;
    private static IPEndPoint serverEndpoint;

    static void Main()
    {
        udpClient = new UdpClient(ClientPort);
        serverEndpoint = new IPEndPoint(IPAddress.Loopback, ServerPort);

        Console.WriteLine("Введите ваш ник:");
        string nickname = Console.ReadLine();

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        while (true)
        {
            string message = Console.ReadLine();
            SendMessage($"{nickname}: {message}");
        }
    }

    private static void ReceiveMessages()
    {
        while (true)
        {
            byte[] data = udpClient.Receive(ref serverEndpoint);
            string receivedMessage = Encoding.UTF8.GetString(data);

            Console.WriteLine($"Сервер: {receivedMessage}");
        }
    }

    private static void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, serverEndpoint);
    }
}