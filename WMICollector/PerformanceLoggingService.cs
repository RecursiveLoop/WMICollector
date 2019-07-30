using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WMICollector
{
    public class PerformanceLoggingService : ServiceBase
    {
        static int Interval = 100;
        static ServiceProvider Container;

        static Task tsk = null;

        static CancellationTokenSource cts = null;

        public PerformanceLoggingService()
        {

        }

        void ExecuteCollection(CancellationToken ct)
        {
            try
            {
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
                            } while (!ct.IsCancellationRequested && lst.Count < 50);
                            if (lst.Count > 0)
                            {
                                csv.WriteRecords(lst);
                                csv.Flush();
                                Console.WriteLine($"Wrote {lst.Count} records.");

                            }
                        } while (!ct.IsCancellationRequested);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            cts = new CancellationTokenSource();

            tsk = Task.Factory.StartNew(() =>
            {
                ExecuteCollection(cts.Token);
            });

        }

        protected override void OnStop()
        {
            if (cts != null)
                cts.Cancel();
            if (tsk != null)
                Task.WaitAll(tsk);
            base.OnStop();
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
