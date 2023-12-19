using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Model
{
    internal class TurnoverHeader
    {
        /// <summary>
        /// Sub header of main header
        /// </summary>
        public class TurnoverSubHeader
        {
            public string Header { get; set; }                                           /// Column name

            public string ColumnSplit1 { get; set; }                                     /// 1 Sub column name
            public string ColumnSplit2 { get; set; }                                     /// 2 Sub column name

            public TurnoverSubHeader(string header, string column1, string column2) 
            { 
                this.Header = header;
                this.ColumnSplit1 = column1;
                this.ColumnSplit2 = column2;
            }

            public TurnoverSubHeader() { }
        }

        public string Column1 { get; private set; }                                      /// 1 Column name

        public TurnoverSubHeader Column2 { get; private set; }                           /// 2 Column with sub columns
        public TurnoverSubHeader Column3 { get; private set; }                           /// 3 Column with sub columns
        public TurnoverSubHeader Column4 { get; private set; }                           /// 4 Column with sub columns


        /// <summary>
        ///     Constructor overload for declaring header names
        /// </summary>
        /// <param name="c1">String First column name</param>
        /// <param name="c2">TurnoverSubHeader for column2 names</param>
        /// <param name="c3">TurnoverSubHeader for column3 names</param>
        /// <param name="c4">TurnoverSubHeader for column4 names</param>
        public TurnoverHeader(string c1, TurnoverSubHeader c2, TurnoverSubHeader c3, TurnoverSubHeader c4) 
        {
            this.Column1 = c1;
            this.Column2 = c2;
            this.Column3 = c3;
            this.Column4 = c4;
        }

        /// <summary>
        ///     Constructor overload for declaring header names
        /// </summary>
        /// <param name="c1">String First column name</param>
        /// <param name="c2">String Array of column, subcolumn, subcolumn2 names</param>
        /// <param name="c3">String Array of column, subcolumn, subcolumn2 names</param>
        /// <param name="c4">String Array of column, subcolumn, subcolumn2 names</param>
        public TurnoverHeader(string c1, string[] c2, string[] c3, string[] c4)
        {
            this.Column1 = c1;
            this.Column2 = new TurnoverSubHeader(c2[0], c2[1], c2[2]);
            this.Column3 = new TurnoverSubHeader(c3[0], c3[1], c3[2]);
            this.Column4 = new TurnoverSubHeader(c4[0], c4[1], c4[2]);
        }

        /// <summary>
        ///     Constructor overload for declaring header names
        /// </summary>
        /// <param name="c1">String column1</param>
        /// <param name="c20">String column2</param>
        /// <param name="c21">String sub column2</param>
        /// <param name="c22">String sub column2</param>
        /// <param name="c30">String column3</param>
        /// <param name="c31">String sub column3</param>
        /// <param name="c32">String sub column3</param>
        /// <param name="c40">String column4</param>
        /// <param name="c41">String sub column4</param>
        /// <param name="c42">String sub column4</param>
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
