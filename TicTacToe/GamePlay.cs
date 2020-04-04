using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TicTacToeLibrary
{
    //define a Service contract for GamePlay class
    [ServiceContract]
    public interface IGame
    {
        [OperationContract]
        void Play(bool player1Try, int cellPosition);
        [OperationContract]
        string GetMark(int cellPosition);
        [OperationContract]
        List<int> CheckWinner();
        [OperationContract]
        void CountScores();
        [OperationContract]
        void CreateNewGame();
        bool GameEnd { [OperationContract] get; }
        bool Player1Turn { [OperationContract] get; [OperationContract] set; }
        int Player1Score { [OperationContract] get; }
        int Player2Score { [OperationContract] get; }

    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GamePlay : IGame
    //public class GamePlay
    {
        private List<Mark> marks = null;
        private bool gameEnd;
        private bool player1Turn;
        private int scorePlayer1 = 0, scorePlayer2 = 0;
        //Constructor
        public GamePlay()
        {
            marks = new List<Mark>();
            CreateNewGame();
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
        public void Play (bool player1Try, int cellPosition)
        {
            //if the selected cell has been empty, then move forwards with player turn and mark set up, or else do nothing
            if (marks[cellPosition].MarkId == Mark.MarkID.Blank)
            {
                //player 1 turn will mark X to blank cell
                if (player1Try)
                {
                    marks[cellPosition] = new Mark(Mark.MarkID.X);
                    player1Turn = false;
                }
                else//player 2 turn will mark O to blank cell
                {
                    marks[cellPosition] = new Mark(Mark.MarkID.O);
                    player1Turn = true;
                }
            }
        }

        public string GetMark(int cellPosition)
        {
            return marks[cellPosition].ToString();
        }

        /// <summary>
        /// this method check winning pattern and return a list of cells that form the that winning pattern
        /// </summary>
        /// <returns></returns>
        public List<int> CheckWinner()
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
                return winners;
            }
            // 3 cells on 2nd horizontal row has same value
            if (marks[3].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[3].MarkId) && (marks[5].MarkId == marks[3].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(3);
                winners.Add(4);
                winners.Add(5);
                return winners;
            }
            // 3 cells on 3rd horizontal row has same value
            if (marks[6].MarkId != Mark.MarkID.Blank && (marks[7].MarkId == marks[6].MarkId) && (marks[8].MarkId == marks[6].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(6);
                winners.Add(7);
                winners.Add(8);
                return winners;
            }

            //check vertical line
            // 3 cells on 1st vertical column has same value
            if (marks[0].MarkId != Mark.MarkID.Blank && (marks[3].MarkId == marks[0].MarkId) && (marks[6].MarkId == marks[0].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(0);
                winners.Add(3);
                winners.Add(6);
                return winners;
            }
            // 3 cells on 2nd vertical column has same value
            if (marks[1].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[1].MarkId) && (marks[7].MarkId == marks[1].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(1);
                winners.Add(4);
                winners.Add(7);
                return winners;
            }
            // 3 cells on 3rd vertical column has same value
            if (marks[2].MarkId != Mark.MarkID.Blank && (marks[5].MarkId == marks[2].MarkId) && (marks[8].MarkId == marks[2].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(2);
                winners.Add(5);
                winners.Add(8);
                return winners;
            }

            //check diagonal line
            // 3 cells on 1st diagonal line (top left to bottom right) has same value
            if (marks[0].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[0].MarkId) && (marks[8].MarkId == marks[0].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(0);
                winners.Add(4);
                winners.Add(8);
                return winners;
            }
            // 3 cells on 2nd diagonal line (top right to bottom left) has same value
            if (marks[2].MarkId != Mark.MarkID.Blank && (marks[4].MarkId == marks[2].MarkId) && (marks[6].MarkId == marks[2].MarkId))
            {
                gameEnd = true;//set game to End
                winners.Add(2);
                winners.Add(4);
                winners.Add(6);
                return winners;
            }

            //if all cells are marked and gameEnd is still false, set the gameEnd to true
            if(!marks.Any(mark => mark.MarkId == Mark.MarkID.Blank)) //no more blank cell
            {
                gameEnd = true;
            }

            return null; //return null list if the cells are all marked
           
        }

        public void CountScores()
        {            
            if (gameEnd && player1Turn)
            {
                scorePlayer2 += 1;
            }
            else if (gameEnd && !player1Turn)
            {
                scorePlayer1 += 1;
            }
        }

        
        public void CreateNewGame()
        {
            //clear all cells in current game
            marks.Clear();
            //create a list of blank cells (9 cells)
            for(int i = 0; i < 9; ++i)
            {
                marks.Add(new Mark(Mark.MarkID.Blank));
            }
            //set player1 play first
            player1Turn = true;
            //set gameEnd
            gameEnd = false;
        }
    }
   
}
