using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class Field
    {
        public Type type;
        public bool visible;
        public bool marked;
        public int value;
        
    }

    enum Type { empty, number, mine }
}
