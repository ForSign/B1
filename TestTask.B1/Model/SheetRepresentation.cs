using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    /// <summary>
    /// Model for sheet representation in TableViewPage
    /// </summary>
    internal class SheetRepresentation
    {
        public TurnoverSheet sheet { get; set; }
        public string id { get; set; }

        public SheetRepresentation(TurnoverSheet sheet, string id)
        {
            this.sheet = sheet;
            this.id = id;
        }
    }
}
