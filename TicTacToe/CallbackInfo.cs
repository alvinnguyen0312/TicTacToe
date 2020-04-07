using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TicTacToeLibrary
{
    [DataContract]
    public class CallbackInfo
    {
        [DataMember]
        public bool GameEnd { get; private set; }
        [DataMember]
        public bool Player1Turn { get; private set; }
        [DataMember]
        public int Player1Score { get; private set; }
        [DataMember]
        public int Player2Score { get; private set; }
        [DataMember]
        public string Result { get; private set; }
        [DataMember]
        public Mark SelectedMark { get; private set; }
        [DataMember]
        public List<int> WinningCells { get; private set; }

        public CallbackInfo(bool gameEnd, bool p1turn, int p1score, int p2score, string result, Mark selectedMark, List<int> winningCells)
        {
            GameEnd = gameEnd;
            Player1Turn = p1turn;
            Player1Score = p1score;
            Player2Score = p2score;
            Result = result;
            SelectedMark = selectedMark;
            WinningCells = winningCells;
        }
    }
}
