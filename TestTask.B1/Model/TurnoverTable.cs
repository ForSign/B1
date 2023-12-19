using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    class TurnoverTable
    {
        public string? ClassName { get; set; }                                                  /// Class name
        public List<TurnoverTableGroup>? AccountGroups { get; set; }                            /// 4 digit account groups

        public TurnoverTableRow? TotalByClass { get; set; }                                     /// Class total

        public TurnoverTable() { }
    }
}
