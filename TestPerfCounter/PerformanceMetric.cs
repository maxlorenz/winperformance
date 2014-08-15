using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestPerfCounter
{
    class PerformanceMetric
    {
        string query;
        int maxValue;

        Label label;
        ProgressBar progress;

        WMIConnector wc;

        public PerformanceMetric(string query, int maxValue, Label label, ProgressBar progress)
        {
            this.query = query;
            this.maxValue = maxValue;
            this.label = label;
            this.progress = progress;

            wc = new WMIConnector();
        }

        public void Update()
        {
            string wmiQueryResult = wc.MakeQuery(this.query)[0];

            bool healthy = int.Parse(wmiQueryResult) <= maxValue;
            Brush color = healthy ? Brushes.LimeGreen : Brushes.OrangeRed;

            this.label.Dispatcher.BeginInvoke((Action)(() =>
            {
                label.Content = wmiQueryResult;
                progress.Value = int.Parse(wmiQueryResult);
                label.Background = color;
                progress.Foreground = color;
            }));
        }

    }
}
