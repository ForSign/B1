using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    class TurnoverTable
    {
        string? ClassName { get; set; }                                                  /// Class name
        List<TurnoverTableGroup>? AccountGroups { get; set; }                            /// 4 digit account groups

        TurnoverTableRow? TotalByClass { get; set; }                                     /// Class total
    }
}
