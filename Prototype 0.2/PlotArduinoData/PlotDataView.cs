namespace PlotArduinoData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using Board;
    using System.Windows.Forms.DataVisualization.Charting;
    using System.Windows.Forms;
    using System.Threading;

    public partial class PlotDataView : Form
    {
        Controller _controller;
        static Series chartSeries1;
        string PORTNAME;
        private const int GRAPHSIZE = 1000;

        private static readonly object locker = new object();

        private List<ushort> lusChA_data = new List<ushort>();

        private Queue<ushort> receivedData = new Queue<ushort>();

        //bool graphModeOn = true;

        public PlotDataView()
        {
            InitializeComponent();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if(cbPortName.SelectedValue != null)
            {
                PORTNAME = cbPortName.SelectedItem.ToString();
                _controller = new Controller(PORTNAME);
                _controller.DataReceived += ControllerOnDataReceived;
                _controller.Open();
                lbSt.Text = "Ok!";
            }
            else
            {
                lbSt.Text = "Selecione uma Porta Externa. Verifique se o seu dispositivo está conectado!";
            }

            if(_controller != null && _controller.IsOpen)
            {
                timerSerial.Interval = 100;
                timerSerial.Enabled = true;
                timerSerial.Start();
                chartSeries1 = new Series();
            }
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            _controller.DataReceived -= ControllerOnDataReceived;
            _controller.Close();
            timerSerial.Stop();
            this.Dispose();
        }

        private void ControllerOnDataReceived(object sender, ControllerDataReceivedEventArgs controllerDataReceivedEventArgs)
        {
            lusChA_data.AddRange(controllerDataReceivedEventArgs.BoardData);
            //controllerDataReceivedEventArgs.BoardData.ToList().ForEach(x => lusChA_data.Enqueue(x));
        }

        private void timerSerial_Tick(object sender, EventArgs e)
        {
            if (lusChA_data.Count > 0)
            {
                this.Invoke(new Action(() => updateChart()));
            }
        }
        
        private void updateChart()
        {
            lock (locker)
            {
                while(lusChA_data.Count>0)
                {
                    //var item = receivedData.ToList().ForEach(x=>x)
                    lusChA_data.ForEach(x => chartSeries1.Points.Add(x));
                    //lusChA_data.ForEach(x => richTextBox1.AppendText(x + "\t"));
                    //chartSeries1.Points.Add(lusChA_data.Select(i => (double)i).ToArray());
                    lusChA_data.Clear();
                }
            }
            
            //chartSeries2.Points.Add(smoothingData.Average());
            //if (chartSeries1.Points.Count >= GRAPHSIZE || chartSeries2.Points.Count >= GRAPHSIZE)
            if (chartSeries1.Points.Count >= GRAPHSIZE )
            {
                chartSeries1.Points.Clear();
                //chartSeries2.Points.Clear();
            }
        }

        private void timerClick_Tick(object sender, EventArgs e)
        {
            timerClick.Enabled = false;
            timerClick.Stop();
        }

        public void chartConfiguration()
        {
            chart.Series.Clear();

            chart.ChartAreas[0].AxisY.Maximum = 1050;
            chart.ChartAreas[0].AxisX.Maximum = 1050;

            chartSeries1 = this.chart.Series.Add("Channel 1 - Raw");
            chartSeries1.ChartType = SeriesChartType.Line;
            //chartSeries2 = this.chart.Series.Add("Channel 2 - Smooth");
            //chartSeries2.ChartType = SeriesChartType.Line;
            //Enumerable.Repeat(0.0, WINDOWSIZE).ToList().ForEach(x => chartSeries1.Points.Add(x));
        }

        private void cbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var ports = System.IO.Ports.SerialPort.GetPortNames();
            //cbPortName.DataSource = ports;
        }

        private void cbPortName_Click(object sender, EventArgs e)
        {
            var ports = System.IO.Ports.SerialPort.GetPortNames();
            cbPortName.DataSource = ports;
        }

    }
}
