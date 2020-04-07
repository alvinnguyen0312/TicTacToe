using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;
using System.ServiceModel;

namespace ConsoleAppTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //IGame game = null;
            //try
            //{
            //// Connect to the Game service
            //ChannelFactory<IGame> channel = new ChannelFactory<IGame>(new NetTcpBinding(),
            //    new EndpointAddress("net.tcp://localhost:13200/TicTacToeLibrary/GameService"));
            //game = channel.CreateChannel();
            //} 
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //    GamePlay game = new GamePlay();
            //    // player 1 mark at cell 4
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 4);
            //    Console.WriteLine($"He mark {game.getMark(4)}");
            //    Display(game);

            //    // player try mark at cell 4 again
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 4);
            //    Console.WriteLine($"He mark {game.getMark(4)}");
            //    Display(game);

            //    // player 2 mark at cell 1
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 1);
            //    Console.WriteLine($"He mark {game.getMark(1)}");
            //    Display(game);

            //    // player 1 mark at cell 2
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 2);
            //    Console.WriteLine($"He mark {game.getMark(2)}");
            //    Display(game);

            //    // player 2 mark at cell 5
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 5);
            //    Console.WriteLine($"He mark {game.getMark(5)}");
            //    Display(game);

            //    // player 1 mark at cell 6
            //    Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    game.Play(game.Player1Turn, 6);
            //    Console.WriteLine($"He mark {game.getMark(6)}");
            //    Display(game);

            //    // player 2 mark at cell 7
            //    //Console.WriteLine($"Player turn: {(game.Player1Turn ? "Player1" : "Player2")}");
            //    //game.Play(game.Player1Turn, 7);
            //    //Console.WriteLine($"He mark {game.getMark(7)}");
            //    //Display(game);
            //}

            ////public static void Display(IGame game)
            //public static void Display(GamePlay game)
            //{
            //    try
            //    {
            //        var result = game.checkWinner();
            //        if (result != null)
            //        {
            //            Console.WriteLine($"Winner is detected: {(game.Player1Turn ? "Player2" : "Player1")}");
            //            foreach (var r in result)
            //            {
            //                Console.WriteLine(r.ToString());
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("Winner is not detected!");
            //        }
            //    }catch(Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }
    }
}
