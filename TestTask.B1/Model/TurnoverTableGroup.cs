using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    class TurnoverTableGroup
    {
        public string? GroupID { get; set; }                                             /// 2 digit group identifier
        public List<TurnoverTableRow>? TableRows { get; set; }                           /// Group rows

        public TurnoverTableGroup() { }
    }
}
