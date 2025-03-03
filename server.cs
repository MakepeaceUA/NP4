using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ConsoleApp37
{

    class GameServer
    {
        static void Main()
        {
            UdpClient server = new UdpClient(5000);
            IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, 5000);

            Console.WriteLine("Сервер запущен и ожидает подключения...");

            while (true)
            {
                byte[] ClientMessage = server.Receive(ref EndPoint);
                string message = Encoding.UTF8.GetString(ClientMessage);
                Console.WriteLine($"Получено сообщение от клиента: {message}");

                if (message == "START")
                {
                    for (int round = 1; round <= 5; round++)
                    {
                        Console.WriteLine($"Раунд {round} начинается...");
                       
                        ClientMessage = server.Receive(ref EndPoint);
                        string PlayerChoice = Encoding.UTF8.GetString(ClientMessage);
                        Console.WriteLine($"Игрок выбрал: {PlayerChoice}");

                        string EnemyChoice = GetChoice();
                        Console.WriteLine($"Противник выбрал: {EnemyChoice}");

                        byte[] response = Encoding.UTF8.GetBytes(EnemyChoice);
                        server.Send(response, response.Length, EndPoint);

                        string res = RoundWinner(PlayerChoice, EnemyChoice);
                        Console.WriteLine(res);
                        byte[] ResMessage = Encoding.UTF8.GetBytes(res);
                        server.Send(ResMessage, ResMessage.Length, EndPoint);
                        Thread.Sleep(1000);
                    }
                    break;
                }
            }
            server.Close();
        }

        static string GetChoice()
        {
            Random rand = new Random();
            int choice = rand.Next(1, 4);
            switch (choice)
            {
                case 1:
                    return "Камень";
                case 2:
                    return "Ножницы";
                case 3:
                    return "Бумага";
                default:
                    return "Ножницы"; 
            }
        }

        static string RoundWinner(string PlayerChoice, string EnemyChoice)
        {
            if (PlayerChoice == EnemyChoice)
            {
                return "Ничья!";
            }
            if ((PlayerChoice == "Камень" && EnemyChoice == "Ножницы") || (PlayerChoice == "Ножницы" && EnemyChoice == "Бумага") || (PlayerChoice == "Бумага" && EnemyChoice == "Камень"))
            {
                return "Игрок победил в раунде!";
            }
            else
            {
                return "Противник победил в раунде!";
            }
        }
    }
}