using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

        /// <summary>
        /// After window loaded set ItemsSource list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            filterList.Add(new Model.FilterModel());
            LV_Filters.ItemsSource = filterList;
        }

        /// <summary>
        /// If there was an example data after GotFocus event accur erase it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GotFocus_ClearExample(object sender, RoutedEventArgs e)
        {
            TextBox? tb = sender as TextBox;

            if (tb?.Text == defaultTxt)
            {
                tb.Text = string.Empty;
                filterList.Add(new Model.FilterModel());
            }
        }

        /// <summary>
        /// Set focus to textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_Filters_GotFocus(object sender, RoutedEventArgs e)
        {
            var a = ForSign.SPM.Library.UIHelpers.GetChildOfType<TextBox>
                ((DependencyObject)e.OriginalSource);

            if (a != null)
                a.Focus();
        }

        /// <summary>
        /// Prompt select files to purge
        /// Then prompt to savefile
        /// Purges them and merges to savefile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
