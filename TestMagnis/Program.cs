using TestMagnis.DataModel;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace TestMagnis
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server server = new Server();
            //server.Setup();
            //server.Start();
            ChatServer server = new ChatServer();
            server.Start();
            Console.ReadLine();
        }
    }
}
