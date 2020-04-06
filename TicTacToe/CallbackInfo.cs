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
        public bool GameEnd { get; set; }
        [DataMember]
        public bool Player1Turn { get; set; }
        [DataMember]
        public int Player1Score { get; set; }
        [DataMember]
        public int Player2Score { get; set; }
        [DataMember]
        public string Result { get; set; }

        public CallbackInfo(bool gameEnd, bool p1turn, int p1score, int p2score, string result)
        {
            GameEnd = gameEnd;
            Player1Turn = p1turn;
            Player1Score = p1score;
            Player2Score = p2score;
            Result = result;
        }
    }
}
