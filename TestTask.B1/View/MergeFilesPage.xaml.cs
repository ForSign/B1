using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestTask.B1.Library;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for MergeFilesPage.xaml
    /// </summary>
    public partial class MergeFilesPage : Window
    {
        ObservableCollection<Model.FilterModel> filterList = new ObservableCollection<Model.FilterModel>();
        private string defaultTxt = (new Model.FilterModel()).filterText;

        public MergeFilesPage()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            filterList.Add(new Model.FilterModel());
            LV_Filters.ItemsSource = filterList;
        }

        private void GotFocus_ClearExample(object sender, RoutedEventArgs e)
        {
            TextBox? tb = sender as TextBox;

            if (tb?.Text == defaultTxt)
            {
                tb.Text = string.Empty;
                filterList.Add(new Model.FilterModel());
            }
        }

        private void LV_Filters_GotFocus(object sender, RoutedEventArgs e)
        {
            var a = ForSign.SPM.Library.UIHelpers.GetChildOfType<TextBox>
                ((DependencyObject)e.OriginalSource);

            if (a != null)
                a.Focus();
        }

        private void Btn_PurgeMerge(object sender, RoutedEventArgs e)
        {
            List<string> filterArray = new List<string>();

            foreach (var filter in filterList)
                if (!String.IsNullOrEmpty(filter.filterText) && filter.filterText != defaultTxt)
                    filterArray.Add(filter.filterText);

            string[]? openFile = FileWorker.OpenFile(Multiselect: true);
            string? saveFile = FileWorker.SaveFile();

            if (openFile is not null && saveFile is not null)
            {
                this.Hide();

                FileWorker.PurgeFiles(filterArray.ToArray(), openFile);
                FileWorker.MergeFiles(openFile, saveFile);
            }

            this.Close(); // Return Control due to ShowDialog();
        }
    }
}
