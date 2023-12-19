using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Interface
{
    interface ITraceTextSink
    {
        void Fail(string msg);
        void Event(string msg, TraceEventType eventType);
    }
}
