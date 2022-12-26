// Server
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server  
{
    class Program
    {
        static void Main(string[] args)
        {
            // Встановлюємо для сокета локальну кінцеву точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Створюємо сокет TСР/ІР
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Призначаємо сокет локальній кінцевій точці і "слухаємо" вхідні сокети
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Починаємо "слухати" з'єднання
                while (true)
                {
                    Console.WriteLine("[!] Expecting a connection through the port {0}", ipEndPoint);

                    // Програма призупиняється, очікуючи вхідне з'єднання
                    Socket handler = sListener.Accept();
                    string login = null;
                    string password = null;

                    // Дочікуємось клієнта, який намагається з нами з'єднатись

                    byte[] log = new byte[1024];
                    int bytesLog = handler.Receive(log);
                    byte[] pass = new byte[1024];
                    int bytesPass = handler.Receive(pass);

                    login += Encoding.UTF8.GetString(log, 0, bytesLog);
                    password += Encoding.UTF8.GetString(pass, 0, bytesPass);

                    // Виводимо його повідомлення в консоль
                    Console.Write("\nReceived message: LOGIN: " + login + ", PASSWORD: " + password + "\n\n");

                    // Відправляємо відповідь клієнту від сервера
                    string reply = "Request processed successfully";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}