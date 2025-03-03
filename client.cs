using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ConsoleApp36
{
    internal class Program
    {
        class GameClient
        {
            static void Main()
            {
                UdpClient client = new UdpClient();
                IPEndPoint EndPoint = new IPEndPoint(IPAddress.Loopback, 5000);

                Console.WriteLine("Подключение к серверу...");
                client.Connect(EndPoint);

                string StartMessage = "START";
                byte[] S_MessageBytes = Encoding.UTF8.GetBytes(StartMessage);
                client.Send(S_MessageBytes, S_MessageBytes.Length);

                for (int round = 1; round <= 5; round++)
                {
                    Console.WriteLine("Выберите: Камень, Ножницы, Бумага");
                    string PlayerChoice = Console.ReadLine();
                    byte[] choice_bytes = Encoding.UTF8.GetBytes(PlayerChoice);
                    client.Send(choice_bytes, choice_bytes.Length);

                    byte[] response = client.Receive(ref EndPoint);
                    string EnemyChoice = Encoding.UTF8.GetString(response);
                    Console.WriteLine($"Противник выбрал: {EnemyChoice}");

                    response = client.Receive(ref EndPoint);
                    string res = Encoding.UTF8.GetString(response);
                    Console.WriteLine(res);

                    Thread.Sleep(1000);
                }
                client.Close();
            }
        }
    }
}