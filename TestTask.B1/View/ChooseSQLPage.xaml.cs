using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestTask.B1.Library;
using TestTask.B1.Model;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for ChooseSQLPage.xaml
    /// </summary>
    public partial class ChooseSQLPage : Window
    {
        private static object ViewControl;
        List<SheetRepresentation> sheet_list = new List<SheetRepresentation>();

        public ChooseSQLPage()
        {
            InitializeComponent();
            Loaded += ChooseSQLPage_Loaded;
        }

        private void ChooseSQLPage_Loaded(object sender, RoutedEventArgs e)
        {
            var dt = new DataTable();
            var db = dbWorker.getInstance();

            dt.Load(db.ExecuteReader("SELECT id, bank_name, sheet_date FROM `B1`.`turnover_sheet`"));
            var rows = dt.AsEnumerable().ToArray();

            rows.Each((row, _) =>
            {
                sheet_list.Add(new SheetRepresentation(
                    new TurnoverSheet
                    {
                        BankName = row[1].ToString(),
                        SheetDate = DateTime.Parse(row[2].ToString())
                    },
                    row[0].ToString()
                ));

            });

            LV_sql.ItemsSource = sheet_list;
        }

        private SheetRepresentation getSelectedModel()
        {
            ListView t;
            try
            {
                t = ViewControl as ListView;
                if (t == null)
                {
                    Trace.WriteLine("Caught NullReferenceException of PasswordControl object");
                    return null;
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Please pick any element first");
                return null;
            }
            return t.SelectedItem as SheetRepresentation;
        }

        private void Btn_OpenView(object sender, RoutedEventArgs e)
        {
            var model = getSelectedModel();
            
            if (model != null)
            {
                string? s = model.id;
                TurnoverSheet sheet = TurnoverLoader.LoadSheetFromDB(Convert.ToInt32(s));

                if (sheet != null)
                {
                    TableViewPage tableViewPage = new TableViewPage(sheet);
                    tableViewPage.Show();
                }
            }
        }

        private void xGotFocus_DB(object sender, RoutedEventArgs e)
        {
            var control = ForSign.SPM.Library.UIHelpers.TryFindParent<ListView>
                ((DependencyObject)e.OriginalSource);
            ViewControl = control;
        }
    }
}
