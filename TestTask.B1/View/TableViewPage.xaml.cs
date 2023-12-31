﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TestTask.B1.Library;
using TestTask.B1.Model;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for SuperPAGE.xaml
    /// </summary>
    public partial class TableViewPage : Window
    {
        TurnoverSheet sheet;
        List<TableRowRepresentation> turnoverTableRows = new List<TableRowRepresentation>() { };

        internal TableViewPage(TurnoverSheet sheet)
        {
            this.sheet = sheet;

            InitializeComponent();
            Loaded += SuperPAGE_Loaded;
        }

        /// <summary>
        /// Sets data for representation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuperPAGE_Loaded(object sender, RoutedEventArgs e)
        {
            /// Sheet MetaData
            this.BankName.Content = sheet.BankName;
            this.Meta.Text = sheet.SheetDescription.Replace(";;", "\n");
            this.Date.Content = sheet.SheetDate.ToString();
            this.Currency.Content = sheet.Currency;

            /// Sheet Headers
            this.Column1.Content = sheet.Header.Column1;

            this.InputBalance.Content = sheet.Header.Column2.Header;
            this.Column2.Content = sheet.Header.Column2.ColumnSplit1;
            this.Column3.Content = sheet.Header.Column2.ColumnSplit2;

            this.Turnover.Content = sheet.Header.Column3.Header;
            this.Column4.Content = sheet.Header.Column3.ColumnSplit1;
            this.Column5.Content = sheet.Header.Column3.ColumnSplit2;

            this.OutputBalance.Content = sheet.Header.Column4.Header;
            this.Column6.Content = sheet.Header.Column4.ColumnSplit1;
            this.Column7.Content = sheet.Header.Column4.ColumnSplit2;

            /// Sheet Rows
            sheet.Tables.Each((table, _) =>
            {
                turnoverTableRows.Add(new TableRowRepresentation( new TurnoverTableRow
                {
                    ID = table.ClassName
                }, "Class"));

                table.AccountGroups.Each((group, _) =>
                {
                    for (int i = 1; i < group.TableRows.Count(); i++)
                    {
                        turnoverTableRows.Add(new TableRowRepresentation(group.TableRows[i], "Normal"));
                    }
                    turnoverTableRows.Add(new TableRowRepresentation(group.TableRows[0], "Bold"));
                });
                turnoverTableRows.Add(new TableRowRepresentation(table.TotalByClass, "Bold"));
            });
            turnoverTableRows.Add(new TableRowRepresentation(sheet.TotalBySheet, "Bold"));

            /// Link ItemSource
            LV_Table.ItemsSource = turnoverTableRows;
        }
    }
}
