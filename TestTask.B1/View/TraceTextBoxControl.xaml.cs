using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TestTask.B1.Interface;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for TraceTextBoxControl.xaml
    /// </summary>
    public partial class TraceTextBoxControl : UserControl, ITraceTextSink
    {
        private delegate void AppendTextDelegate(string msg);

        private TraceListener _listener;

        public TraceTextBoxControl()
        {
            AutoAttach = true;
            InitializeComponent();
        }

        public bool AutoAttach { get; set; }

        public void Event(string msg, TraceEventType eventType)
        {
            AppendText(msg);
        }

        private void AppendText(string msg)
        {
            if (Dispatcher.CheckAccess())
            {
                textBox1.AppendText(msg);
                textBox1.ScrollToEnd();
            }
            else
            {
                Dispatcher.Invoke(new AppendTextDelegate(AppendText), DispatcherPriority.Normal, msg);
            }
        }

        public void Fail(string msg)
        {
            AppendText(msg);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoAttach && _listener == null)
            {
                _listener = new Library.TraceTextSource(this);
                Trace.Listeners.Add(_listener);
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_listener != null)
            {
                Trace.Listeners.Remove(_listener);
                _listener.Dispose();
                _listener = null;
            }
        }
    }
}
