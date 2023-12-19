using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    class TurnoverTableGroup
    {
        string? GroupID { get; set; }                                                    /// 2 digit group identifier
        List<TurnoverTableRow>? TableRows { get; set; }                                  /// Group rows
    }
}
