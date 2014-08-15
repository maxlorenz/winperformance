using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestPerfCounter
{
    class PerformanceMetric
    {
        public string query;
        public int maxValue;
        public Label label;
        public ProgressBar progress;

        private WMIConnector wc;

        public PerformanceMetric(string query, int maxValue, Label label, ProgressBar progress)
        {
            this.query = query;
            this.maxValue = maxValue;
            this.label = label;
            this.progress = progress;

            this.wc = new WMIConnector();
        }

        public void Update()
        {
            string wmiQueryResult = wc.MakeQuery(this.query)[0];
            giveColorFeedback(int.Parse(wmiQueryResult) <= maxValue);

            this.label.Dispatcher.BeginInvoke((Action)(() =>
            {
                label.Content = wmiQueryResult;
                progress.Value = int.Parse(wmiQueryResult);
            }));
        }

        private void giveColorFeedback(bool healthy)
        {
            Brush color = healthy ? Brushes.LimeGreen : Brushes.OrangeRed;

            label.Dispatcher.BeginInvoke((Action)(() =>
            {
                label.Background = color;
                progress.Foreground = color;
            }));
        }

    }
}
