using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homework.helper
{
    public class Column: System.Attribute
    {
        public int ColumnIndex { get; set; }

        public Column(int column)
        {
            ColumnIndex = column;
        }
    }
}
