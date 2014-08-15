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
        WMIConnector wc;

        public MainWindow()
        {
            InitializeComponent();

            wc = new WMIConnector();

            Thread perfCounter = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    perfCount();
                }
            }));

            perfCounter.IsBackground = true;
            perfCounter.Start();
        }

        /*
         * Performance measured by querying the WMI.
         * See here for additional info
         * http://serverfault.com/questions/54753/common-wql-monitoring-queries
         * 
         */
        public void perfCount()
        {
            string processorQueueLength =   wc.MakeQuery("select ProcessorQueueLength from Win32_PerfFormattedData_PerfOS_System")[0];
            string pagesInputPerSecond =    wc.MakeQuery("select PagesInputPerSec from Win32_PerfFormattedData_PerfOS_Memory")[0];
            string diskQueueLength =        wc.MakeQuery("select AvgDiskQueueLength from Win32_PerfFormattedData_PerfDisk_PhysicalDisk where name = '_Total'")[0];
            string outputQueueLength =      wc.MakeQuery("select OutputQueueLength from Win32_PerfFormattedData_Tcpip_NetworkInterface")[0];
            string packetsReceivedErrors =  wc.MakeQuery("select PacketsReceivedErrors from Win32_PerfFormattedData_Tcpip_NetworkInterface")[0];

            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                giveColorFeedback(txtProcessorQueueLength,      int.Parse(processorQueueLength)  < 10);
                giveColorFeedback(txtPagesInputPerSec,          int.Parse(pagesInputPerSecond)   < 10);
                giveColorFeedback(txtAverageDiskQueueLength,    int.Parse(diskQueueLength)       <= 2);
                giveColorFeedback(txtOutputQueueLength,         int.Parse(outputQueueLength)     == 0);
                giveColorFeedback(txtPacketsReceivedErrors,     int.Parse(packetsReceivedErrors) == 0);

                txtProcessorQueueLength.Content     = processorQueueLength;
                txtPagesInputPerSec.Content         = pagesInputPerSecond;
                txtAverageDiskQueueLength.Content   = diskQueueLength;
                txtOutputQueueLength.Content        = outputQueueLength;
                txtPacketsReceivedErrors.Content    = packetsReceivedErrors;
            }));
           
        }

        private void giveColorFeedback(Label label, bool healthy)
        {
            label.Background = healthy ? Brushes.LimeGreen : Brushes.OrangeRed;
        }
    }
}
