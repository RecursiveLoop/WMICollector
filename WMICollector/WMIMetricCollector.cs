using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.Linq;
using System.Dynamic;

namespace WMICollector
{
    public class WMIMetricCollector : IMetricCollector
    {
        public void Dispose()
        {

        }

        public dynamic GetData()
        {
            ManagementScope oMs = new System.Management.ManagementScope("root\\cimv2");
            //Inventory
            ObjectQuery InventQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher Inventsearcher = new ManagementObjectSearcher(oMs, InventQuery);
            ManagementObjectCollection InventCollection = Inventsearcher.Get();
            dynamic returnVal = new ExpandoObject();
            returnVal.TimeStamp = DateTime.Now;

            foreach (ManagementObject result in InventCollection)
            {
                returnVal.FreePhysicalMemory = (UInt64)result["FreePhysicalMemory"];
                returnVal.FreeVirtualMemory = (UInt64)result["FreeVirtualMemory"];
                returnVal.TotalVirtualMemorySize = (UInt64)result["TotalVirtualMemorySize"];
                returnVal.TotalVisibleMemorySize = (UInt64)result["TotalVisibleMemorySize"];
                break;
            }

            InventQuery = new ObjectQuery("select * from Win32_PerfFormattedData_PerfOS_Processor");
            Inventsearcher = new ManagementObjectSearcher(oMs, InventQuery);
            InventCollection = Inventsearcher.Get();

            List<double> cpuTimes = new List<double>();

            foreach (var cpuProp in InventCollection.Cast<ManagementObject>())
            {
                var cpuVal = Convert.ToDouble(cpuProp["PercentProcessorTime"]);
                cpuTimes.Add(cpuVal);
            }

            returnVal.CpuUsage = cpuTimes.Average();

            // Disk performance
            ObjectQuery PerfQuery = new ObjectQuery("SELECT * FROM Win32_PerfRawData_PerfDisk_PhysicalDisk");
            ManagementObjectSearcher Perfsearcher = new ManagementObjectSearcher(oMs, PerfQuery);
            ManagementObjectCollection PerfCollection = Perfsearcher.Get();

            UInt64 maxDiskWriteBytesPerSec = PerfCollection.Cast<ManagementObject>().Max(mo => (UInt64)mo["DiskWriteBytesPerSec"]);
            UInt64 maxDiskReadBytesPerSec = PerfCollection.Cast<ManagementObject>().Max(mo => (UInt64)mo["DiskReadBytesPerSec"]);

            returnVal.DiskWriteBytesPerSec = maxDiskWriteBytesPerSec;
            returnVal.DiskReadBytesPerSec = maxDiskReadBytesPerSec;


            PerfQuery = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
            Perfsearcher = new ManagementObjectSearcher(oMs, PerfQuery);
            PerfCollection = Perfsearcher.Get();

            UInt64 totalBytesReceived = Convert.ToUInt64(PerfCollection.Cast<ManagementObject>().Sum(mo =>(long?) Convert.ToInt64( mo["BytesReceivedPerSec"])).GetValueOrDefault(0));
            UInt64 totalBytesSent = Convert.ToUInt64(PerfCollection.Cast<ManagementObject>().Sum(mo => (long?)Convert.ToInt64(mo["BytesSentPerSec"])).GetValueOrDefault(0));

            returnVal.TotalBytesReceived = totalBytesReceived;
            returnVal.TotalBytesSent = totalBytesSent;


            return returnVal;
        }
    }
}
