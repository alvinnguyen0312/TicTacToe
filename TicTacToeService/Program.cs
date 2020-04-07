using System;

// 1. This is to host Shoe object
using System.ServiceModel; // GamePlay and IGame types
using TicTacToeLibrary; // WCF types

namespace TicTacToeService
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2. Register the service address
            ServiceHost servHost = null;
            try
            {
                // 2.a: Register the service base Address
                //servHost = new ServiceHost(typeof(GamePlay), new Uri("net.tcp://localhost:13203/TicTacToeLibrary/"));

                // 2.b and c: Register the service Contract and Binding
                //servHost.AddServiceEndpoint(typeof(IGame), new NetTcpBinding(), "GameService");

                // 2.a , 2.b and c is now implemented in App.config file.
                servHost = new ServiceHost(typeof(GamePlay));

                // 3. Run the service
                servHost.Open();
                Console.WriteLine("Service started. Please any key to quit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Wait for a keystroke
                Console.ReadKey();

                // Shut down
                if (servHost != null)
                    servHost.Close();
            }
        }
    }
}
