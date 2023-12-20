using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
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
