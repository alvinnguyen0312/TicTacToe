using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TicTacToeLibrary
{
    [DataContract]
    public class Mark
    {
        //Define mark types
        public enum MarkID { X, O, Blank};

        [DataMember]
        //Public methods and properties
        public MarkID MarkId { get; private set; }
        [DataMember]
        public int CellPosition { get; private set; }

        public override string ToString()
        {
            return MarkId.ToString();
        }
        //Constructor
        internal Mark(MarkID markID, int cellPosition)
        {
            MarkId = markID;
            CellPosition = cellPosition;
        }

    }
}
