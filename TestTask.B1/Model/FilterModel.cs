using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTask.B1.Model
{
    /// <summary>
    /// Model to represent filter in task1
    /// Thus allowong us to make it dynamic
    /// </summary>
    internal class FilterModel
    {
        public string filterText { get; set; }

        public FilterModel(string filterText = "Enter new filter, for example: ''abc''")
        {
            this.filterText = filterText;
        }
    }
}
