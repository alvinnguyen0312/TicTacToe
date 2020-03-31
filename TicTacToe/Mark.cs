using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    public class Mark
    {
        //Define mark types
        public enum MarkID { X, O, Blank};

        //Public methods and properties
        public MarkID MarkId { get; private set; }

        public override string ToString()
        {
            return MarkId.ToString();
        }

        //Constructor
        internal Mark(MarkID markID)
        {
            MarkId = markID;
        }

    }
}
