using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace WMICollector
{
    public class PerformanceLoggingService : ServiceBase
    {
        static int Interval = 100;
        static ServiceProvider Container;
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Console.WriteLine("Metric Collector and Logger by Amazon Web Services");
            StartUp();
            using (var metricCollector = Container.GetService<IMetricCollector>())
            {
                var strCsvPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");

                using (var writer = new StreamWriter(strCsvPath))
                using (var csv = new CsvWriter(writer))
                {
                    Console.WriteLine($"Writing records to {strCsvPath}");
                    do
                    {
                        List<Object> lst = new List<object>();
                        do
                        {
                            var obj = metricCollector.GetData();

                            lst.Add(obj);

                            Thread.Sleep(Interval);
                        } while (lst.Count < 50);
                        if (lst.Count > 0)
                        {
                            csv.WriteRecords(lst);
                            csv.Flush();
                            Console.WriteLine($"Wrote {lst.Count} records.");

                        }
                    } while (true);
                }
            }
        }

        public void ConsoleStart(string[] args)
        {
            this.OnStart(args);
        }

        public void ConsoleStop()
        {
            this.OnStop();
        }

        static void StartUp()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IMetricCollector, WMIMetricCollector>();
            Container = serviceCollection.BuildServiceProvider();
        }
    }
}
