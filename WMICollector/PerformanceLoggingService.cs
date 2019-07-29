using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace WMICollector
{
    public class PerformanceLoggingService:ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }
    }
}
