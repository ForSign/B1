using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    /// <summary>
    /// Model for row representation
    /// </summary>
    class TableRowRepresentation
    {
        public TurnoverTableRow row { get; set; }
        public string Route { get; set; }

        public TableRowRepresentation(TurnoverTableRow orw, string route) 
        {
            this.row = orw;
            this.Route = route;
        }
    }
}
