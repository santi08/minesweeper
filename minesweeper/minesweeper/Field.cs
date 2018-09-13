﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    /// <summary>
    /// This class contains properties of a field
    /// </summary>
    class Field
    {
        public Type type;
        public bool visible;
        public bool marked;
        public int value;
        
    }

    /// <summary>
    /// Describe the type of a fiel
    /// </summary>
    enum Type { empty, number, mine }
}
