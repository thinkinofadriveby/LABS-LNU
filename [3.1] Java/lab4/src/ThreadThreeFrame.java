using System;
        using System.Linq;
        using System.Net;
        using System.Net.Sockets;
        using System.Text;

        namespace Server
        {
class Program
{
    static void Main(string[] args)
    {
        Socket listener = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
        IPAddress addr = IPAddress.Parse("127.0.0.1");
        listener.Bind(new IPEndPoint(addr, 8084));
        listener.Listen(666);
        Console.WriteLine("Listening...");

        Socket handler = listener.Accept();
        Console.WriteLine("Client connected.");

        while (true)
        {
            byte[] msgBuffer = new byte[1024];
            handler.Receive(msgBuffer);

            string msg = Encoding.ASCII.GetString(msgBuffer).Replace("\0", "");
            Console.WriteLine($"Data received: { msg }");

            int result = 0;
            try
            {
                var numbers = msg.Split(' ');
                if (numbers.Length == 0)
                    throw new FormatException("Incorrect data format.");

                result += numbers.Aggregate(1, (i, s) => i * int.Parse(s));
            }
            catch (FormatException exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
                handler.Send(Encoding.ASCII.GetBytes(
                        $"Encountered error from server: {exception.Message}"));
                continue;
            }
            Console.WriteLine($"Send result: { result }");
            handler.Send(Encoding.ASCII.GetBytes($"Result: { result }"));
        }
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
}
}