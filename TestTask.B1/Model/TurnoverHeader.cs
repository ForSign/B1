using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverHeader
    {
        public class TurnoverSubHeader
        {
            public string Header { get; set; }

            public string ColumnSplit1 { get; set; }
            public string ColumnSplit2 { get; set; }

            public TurnoverSubHeader(string header, string columnSplit1, string columnSplit2) 
            { 
                this.Header = header;
                this.ColumnSplit1 = columnSplit1;
                this.ColumnSplit2 = columnSplit2;
            }

            public TurnoverSubHeader() { }
        }

        public string Column1 { get; private set; }

        public TurnoverSubHeader Column2 { get; private set; }
        public TurnoverSubHeader Column3 { get; private set; }
        public TurnoverSubHeader Column4 { get; private set; }

        public TurnoverHeader(string c1, TurnoverSubHeader c2, TurnoverSubHeader c3, TurnoverSubHeader c4) 
        {
            this.Column1 = c1;
            this.Column2 = c2;
            this.Column3 = c3;
            this.Column4 = c4;
        }

        public TurnoverHeader(string c1, string[] c2, string[] c3, string[] c4)
        {
            this.Column1 = c1;
            this.Column2 = new TurnoverSubHeader(c2[0], c2[1], c2[2]);
            this.Column3 = new TurnoverSubHeader(c3[0], c3[1], c3[2]);
            this.Column4 = new TurnoverSubHeader(c4[0], c4[1], c4[2]);
        }

        public TurnoverHeader(string c1, 
            string c20, string c21, string c22, 
            string c30, string c31, string c32,
            string c40, string c41, string c42)
        {
            this.Column1 = c1;
            this.Column2 = new TurnoverSubHeader(c20, c21, c22);
            this.Column3 = new TurnoverSubHeader(c30, c31, c32);
            this.Column4 = new TurnoverSubHeader(c40, c41, c42);
        }

    }
}
