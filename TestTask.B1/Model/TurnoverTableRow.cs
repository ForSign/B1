using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverTableRow
    {
        public string? ID { get; set; }                                                  /// Table's B/ca aka account identifier
        public string? InputBalanceActive { get; set; }                                  /// 1 Column Data
        public string? InputBalancePassive { get; set; }                                 /// 2 Column Data
        public string? DebitTurnover { get; set; }                                       /// 3 Column Data
        public string? LoanTurnover { get; set; }                                        /// 4 Column Data
        public string? OutputBalanceActive { get; set; }                                 /// 5 Column Data
        public string? OutputBalancePassive { get; set; }                                /// 6 Column Data

        public TurnoverTableRow() { }
    }
}
