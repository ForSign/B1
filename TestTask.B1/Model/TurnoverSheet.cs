using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverSheet
    {
        public string BankName { get; set; } = "No entry set";
        public string SheetDescription { get; set; } = "No entry set";
        public DateTime SheetDate { get; set; } = DateTime.MinValue;

        public TurnoverHeader? Header { get; set; }
        public List<TurnoverTable>? turnoverTables { get; set; }

        public string[]? ClassNames { get; set; }
        public string[]? TotalByClass {  get; set; }

        public TurnoverTable? TotatlBySheet { get; set; }

        public TurnoverSheet()
        {

        }
    }
}
