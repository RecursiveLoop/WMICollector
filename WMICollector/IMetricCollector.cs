using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WMICollector
{
    public interface IMetricCollector : IDisposable
    {
        dynamic GetData();
    }
}
