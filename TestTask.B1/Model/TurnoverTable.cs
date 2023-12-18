using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverTable
    {
        public string? ID { get; set; }
        public string? InputBalanceActive { get; set; }
        public string? InputBalancePassive { get; set; }
        public string? DebitTurnover { get; set; }
        public string? LoanTurnover { get; set; }
        public string? OutputBalanceActive { get; set; }
        public string? OutputBalancePassive { get; set; }

        public TurnoverTable() 
        {
            
        }
    }
}
