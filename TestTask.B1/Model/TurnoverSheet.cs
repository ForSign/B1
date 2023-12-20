using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverSheet
    {
        public string? BankName { get; set; }                                            /// Bank name
        public string? SheetDescription { get; set; }                                    /// Globlal description of document
        public string? Currency { get; set; }                                            /// Currency type
        public DateTime SheetDate { get; set; } = DateTime.MinValue;                     /// Date of document

        public TurnoverHeader? Header { get; set; }                                      /// Table column header names
        public List<TurnoverTable>? Tables { get; set; }                                 /// Class tables (exmple: 1 - 9)

        public TurnoverTableRow? TotalBySheet { get; set; }                              /// Total balance row

        public TurnoverSheet() { }
    }
}
