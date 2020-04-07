using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;

namespace TicTacToeLibrary
{
    //define a callback contract for client to implement
    [ServiceContract]
    public interface ICallback // Data from Client to Service
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGameUI(CallbackInfo info);
    }

    // Define a Service contract for GamePlay class
    // And link two ServiceContract together to communicate.
    [ServiceContract(CallbackContract = typeof(ICallback))] // Data from Service to Client
    public interface IGame
    {

        [OperationContract]
        Mark Play(bool player1Try, int cellPosition);
        //[OperationContract]
        //string GetMark(int cellPosition);
        [OperationContract(IsOneWay = true)]
        void CheckWinner();
        [OperationContract(IsOneWay = true)]
        void CreateNewGame();
        [OperationContract(IsOneWay = true)]
        void Repopulate();
        bool GameEnd { [OperationContract]get; }
        bool Player1Turn { [OperationContract]get; }
        int Player1Score { [OperationContract]get; }
        int Player2Score { [OperationContract]get; }
        [OperationContract(IsOneWay = true)]
        void RegisterForCallbacks();
        [OperationContract(IsOneWay = true)]
        void UnregisterFromCallbacks();

    }

    // To use the same object when the two clients execute
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GamePlay : IGame
    //public class GamePlay
    {
        private List<Mark> marks = null;
        private bool gameEnd;
        private bool player1Turn;
        private int scorePlayer1 = 0, scorePlayer2 = 0;
        private string result = "";
        private static uint objCount = 0;
        private uint objNum;
        private List<int> winningCells = null;
        private Mark selectedMark = null;

        // HashSet requre a unique value
        // It holds bunch of client objects
        private HashSet<ICallback> callbacks = new HashSet<ICallback>();

        //Constructor
        public GamePlay()
        {
            objNum = ++objCount;
            Console.WriteLine($"Creating GamePlay Object #{objNum}");

            marks = new List<Mark>();
            Repopulate();
        }

        // A client calls this method when it's loading!
        public void RegisterForCallbacks()
        {       
            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            // // If there is no duplicate, add the client's callback (proxy) object to the collection
            if (!callbacks.Contains(cb))
                callbacks.Add(cb);
        }

        public void UnregisterFromCallbacks()
        {
            // A client calls this method when it's quitting!

            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            // Remove the client's callback object from the collection
            // so that the Shoe object won't try to call a method on a 
            // "dangling reference"
            if (callbacks.Contains(cb))
                callbacks.Remove(cb);
        }

        /// <summary>
        /// get the game end status
        /// </summary>
        public bool GameEnd
        {
            get { return gameEnd; }
        }

        /// <summary>
        /// get,set the turn of first player
        /// </summary>
        public bool Player1Turn
        {
            get { return player1Turn; }
            set
            {
                player1Turn = value;
            }
        }
        /// <summary>
        /// get score of first player
        /// </summary>
        public int Player1Score
        {
            get { return scorePlayer1; }
        }
        /// <summary>
        /// get score of secondplayer
        /// </summary>
        public int Player2Score
        {
            get { return scorePlayer2; }
        }


        

        /// <summary>
        /// Method play() will set player turn and mark type 
        /// </summary>
        /// <param name="player1Try"></param>
        /// <param name="cellPosition"></param>
        public Mark Play (bool player1Try, int cellPosition)
        {
            
            //if the selected cell has been empty, then move forwards with player turn and mark set up, or else do nothing
            if (marks[cellPosition].MarkId == Mark.MarkID.Blank)
            {
                
                //player 1 turn will mark X to blank cell
                if (player1Try)
                {
                    marks[cellPosition] = new Mark(Mark.MarkID.X, cellPosition);
                    player1Turn = false;
                }
                else//player 2 turn will mark O to blank cell
                {
                    marks[cellPosition] = new Mark(Mark.MarkID.O, cellPosition);
                    player1Turn = true;
                }
                //updateAllClients(gameEnd);

                Console.WriteLine($"GamePlay Object #{objNum} Playing with {marks[cellPosition]}.");
            }
            else
            {
                // If the selected cell has been used, then return null.
                return null;
            }


            selectedMark = marks[cellPosition];

            return marks[cellPosition];
        }

        //public string GetMark(int cellPosition)
        //{
        //    return marks[cellPosition].ToString();
        //}

        /// <summary>
        /// this method check winning pattern and return a list of cells that form the that winning pattern
        /// </summary>
        /// <returns></returns>
        public void CheckWinner()
        {
            List<int> winners = new List<int>();
            //check horizontal line
            // 3 cells on 1st horizontal row has same value
            if(marks[0].MarkId != Mark.MarkID.Blank && (marks[1].MarkId == marks[0].MarkId) && (marks[2].MarkId == marks[0].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(0);
                winners.Add(1);
                winners.Add(2);
                //return winners;
            }
            // 3 cells on 2nd horizontal row has same value
            if (marks[3].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[3].MarkId) && (marks[5].MarkId == marks[3].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(3);
                winners.Add(4);
                winners.Add(5);
                //return winners;
            }
            // 3 cells on 3rd horizontal row has same value
            if (marks[6].MarkId != Mark.MarkID.Blank && (marks[7].MarkId == marks[6].MarkId) && (marks[8].MarkId == marks[6].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(6);
                winners.Add(7);
                winners.Add(8);
                //return winners;
            }

            //check vertical line
            // 3 cells on 1st vertical column has same value
            if (marks[0].MarkId != Mark.MarkID.Blank && (marks[3].MarkId == marks[0].MarkId) && (marks[6].MarkId == marks[0].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(0);
                winners.Add(3);
                winners.Add(6);
                //return winners;
            }
            // 3 cells on 2nd vertical column has same value
            if (marks[1].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[1].MarkId) && (marks[7].MarkId == marks[1].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(1);
                winners.Add(4);
                winners.Add(7);
                //return winners;
            }
            // 3 cells on 3rd vertical column has same value
            if (marks[2].MarkId != Mark.MarkID.Blank && (marks[5].MarkId == marks[2].MarkId) && (marks[8].MarkId == marks[2].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(2);
                winners.Add(5);
                winners.Add(8);
                //return winners;
            }

            //check diagonal line
            // 3 cells on 1st diagonal line (top left to bottom right) has same value
            if (marks[0].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[0].MarkId) && (marks[8].MarkId == marks[0].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(0);
                winners.Add(4);
                winners.Add(8);
                //return winners;
            }
            // 3 cells on 2nd diagonal line (top right to bottom left) has same value
            if (marks[2].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[2].MarkId) && (marks[6].MarkId == marks[2].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(2);
                winners.Add(4);
                winners.Add(6);
                //return winners;
            }

            // If all cells are marked and gameEnd is still false, set the gameEnd to true
            if(!marks.Any(mark => mark.MarkId == Mark.MarkID.Blank)) //no more blank cell
            {
                result = "Tie!";
                gameEnd = true;
            }

            if (winners.Count != 0)
            {
                Console.WriteLine($"GamePlay Object #{objNum} Won.");
                CountScores();
            }

            winningCells = winners;

            UpdateAllClients(gameEnd);          

            //return winners; //return null list if the cells are all marked
           
        }

        public void CountScores()
        {
            if (gameEnd && player1Turn)
            {
                scorePlayer2 += 1;
                result = "Player 2 Won!";
            }
            else if (gameEnd && !player1Turn)
            {
                scorePlayer1 += 1;
                result = "Player 1 Won!";
            }
            //updateAllClients(gameEnd);
        }


        public void Repopulate()
        {
            //clear all cells in current game
            marks.Clear();
            //create a list of blank cells (9 cells)
            for(int i = 0; i < 9; ++i)
            {
                marks.Add(new Mark(Mark.MarkID.Blank, i));
            }


            // Resets member variables
            player1Turn = true;
            result = "";
            gameEnd = false;
            selectedMark = null;

            UpdateAllClients(gameEnd);
        }

        public void CreateNewGame()
        {
            Console.WriteLine($"GamePlay Object #{objNum} left.");
            Repopulate();
            scorePlayer1 = 0;
            scorePlayer2 = 0;
        }



        //helper method
        private void UpdateAllClients(bool gameEnd)
        {

            CallbackInfo info = new CallbackInfo(gameEnd, player1Turn, scorePlayer1, scorePlayer2, result, selectedMark, winningCells);

            foreach (ICallback cb in callbacks)
                if (cb != null)
                    cb.UpdateGameUI(info);
        }
    }
   
}
