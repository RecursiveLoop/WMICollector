using System;
using System.Collections.Generic;
using System.Text;

namespace WMICollector
{
    public class PerfLogEntry
    {
        public DateTime TimeStamp { get; set; }
        public UInt32 DiskWritesPerSec { get; set; }

        public UInt32 DiskReadsPerSec { get; set; }

        public UInt64 TotalVisibleMemorySize { get; set; }
        public UInt64 FreePhysicalMemory { get; set; }

        public UInt64 TotalVirtualMemorySize { get; set; }
        public UInt64 FreeVirtualMemory { get; set; }

        public double CpuUsage { get; set; }


    }
}
