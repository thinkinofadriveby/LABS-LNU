//  Client
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client 
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    SendMessageFromClient(11000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        static void SendMessageFromClient(int port)
        {
            byte[] bytes = new byte[1024];

            // З'єднуємось з віддаленим пристроєм

            // Встановлюємо віддалену точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // З'єднуємо сокет з віддаленою точкою
            sender.Connect(ipEndPoint);

            string correctLogin = "Nazar Yuras";
            string correctPass = "123456";

            Console.Write("Input login: ");
            string login = Console.ReadLine();

            Console.Write("Input password: ");
            string pass = Console.ReadLine();

            //Console.WriteLine("\nConnecting with {0} ", sender.RemoteEndPoint.ToString());
            byte[] log = Encoding.UTF8.GetBytes(login);
            byte[] password = Encoding.UTF8.GetBytes(pass);

            // Відправляємо дані через сокет
            if (login == correctLogin && pass == correctPass)
            {
                int loginSent = sender.Send(log);
                int passSent = sender.Send(password);
                Console.WriteLine($"\n[!] Server: Hello, {login}");
            }
            else
            {
                Console.WriteLine("Invalid input!");
            }

            // Отримуємо відповідь від сервера
            int bytesRec = sender.Receive(bytes);

            Console.WriteLine("[!] Server: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            if (login != correctLogin || pass != correctPass)
                SendMessageFromClient(port);

            // Звільнюємо сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}