using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestPerfCounter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PerformanceMetric[] metrics;

        public MainWindow()
        {
            InitializeComponent();

            /*
             * Performance measured by querying the WMI.
             * See here for additional info
             * http://serverfault.com/questions/54753/common-wql-monitoring-queries
             * 
             */

            metrics = new PerformanceMetric[] {
                new PerformanceMetric("select ProcessorQueueLength from Win32_PerfFormattedData_PerfOS_System", 
                    10, txtProcessorQueueLength, pgrProcessorQueueLength),
                new PerformanceMetric("select PagesInputPerSec from Win32_PerfFormattedData_PerfOS_Memory", 
                    0, txtPagesInputPerSec, pgrPagesInputPerSec),
                new PerformanceMetric("select AvgDiskQueueLength from Win32_PerfFormattedData_PerfDisk_PhysicalDisk where name = '_Total'", 
                    2, txtAverageDiskQueueLength, pgrAverageDiskQueueLength),
                new PerformanceMetric("select OutputQueueLength from Win32_PerfFormattedData_Tcpip_NetworkInterface", 
                    0, txtOutputQueueLength, pgrOutputQueueLength),
                new PerformanceMetric("select PacketsReceivedErrors from Win32_PerfFormattedData_Tcpip_NetworkInterface", 
                    0, txtPacketsReceivedErrors, pgrPacketsReceivedErrors)
            };

            Thread perfCounter = new Thread(new ThreadStart(perfCount));
            perfCounter.IsBackground = true;
            perfCounter.Start();
        }

        public void perfCount()
        {
            while (true)
            {
                foreach (PerformanceMetric pm in metrics)
                {
                    pm.Update();
                }
            }
        }
    }
}
