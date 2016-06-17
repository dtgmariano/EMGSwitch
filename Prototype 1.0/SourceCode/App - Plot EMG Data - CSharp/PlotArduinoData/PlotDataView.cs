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
    //using System.Threading;

    public partial class PlotDataView : Form
    {
        Controller _controller;
        static Series chartSeries1, chartSeries2;
        string PORTNAME;
        static int GRAPHSIZE;
        static int THRESHOLD;
        static int CLICKDELAY;
        static int SERIALT;
        static int QUEUESIZE;
        static int WINDOWSIZE;

        //List<double> rawSignal;

        private Queue<ushort> receivedData = new Queue<ushort>();
        private Queue<double> smoothingData = new Queue<double>();
        bool graphModeOn = true;
        bool mouseClickModeOn = true;

        public PlotDataView()
        {
            InitializeComponent();
        }
        
        private void bStart_Click(object sender, EventArgs e)
        {
            //PORTNAME = cbPortName.SelectedText;
            
            GRAPHSIZE = Convert.ToInt32(numGraphSize.Value);
            THRESHOLD = Convert.ToInt32(numThreshold.Value);
            CLICKDELAY = Convert.ToInt32(numDelay.Value);
            SERIALT = Convert.ToInt32(numSF.Value);
            QUEUESIZE = Convert.ToInt32(numWindowSize.Value);
            WINDOWSIZE = Convert.ToInt32(numSmoothSize.Value);

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
                chartConfiguration();

                timerSerial.Interval = SERIALT;
                timerSerial.Enabled = true;
                timerSerial.Start();

                bStart.Enabled = false;
                cbPortName.Enabled = false;
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
            Console.WriteLine("");
            //receivedData.Enqueue(controllerDataReceivedEventArgs.BoardData[0]);
            //processQueue();
        }

        private void processQueue()
        {
            while (receivedData.Count > QUEUESIZE)
            { var lostData = receivedData.Dequeue(); }
        }


        private void timerSerial_Tick(object sender, EventArgs e)
        {
            if (receivedData.Count > 0)
            {
                var item = receivedData.Dequeue();

                smoothingData.Enqueue(item);

                if(graphModeOn)
                    this.Invoke(new Action(() => updateChart(item)));

                if(!timerClick.Enabled)
                    this.Invoke(new Action(() => processSignal(item)));

                while(smoothingData.Count >= WINDOWSIZE)
                {
                    smoothingData.Dequeue();
                }
                
            }
        }
        
        private void updateChart(double data)
        {
            chartSeries1.Points.Add(data);
            chartSeries2.Points.Add(smoothingData.Average());
            if (chartSeries1.Points.Count >= GRAPHSIZE || chartSeries2.Points.Count >= GRAPHSIZE)
            {
                chartSeries1.Points.Clear();
                chartSeries2.Points.Clear();
            }
        }

        private void processSignal(double data)
        {
            var threshold = THRESHOLD;

            if (data > threshold)
            {
                timerClick.Interval = CLICKDELAY;
                timerClick.Enabled = true;
                timerClick.Start();

                if (mouseClickModeOn)
                    MouseHandler.DoMouseClick();

                richTextBox1.AppendText(data + " " + threshold + "\n");
                richTextBox1.ScrollToCaret();
                pictureBox1.BackColor = Color.Red;
            }
            else
                pictureBox1.BackColor = Color.AntiqueWhite;
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
            chartSeries2 = this.chart.Series.Add("Channel 2 - Smooth");
            chartSeries2.ChartType = SeriesChartType.Line;
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

        private void cbGraph_CheckedChanged(object sender, EventArgs e)
        {
            if(!cbGraph.Checked)
            {
                graphModeOn = false;
            }
            else
            {
                graphModeOn = true;
            }
        }

        private void numThreshold_ValueChanged(object sender, EventArgs e)
        {
            THRESHOLD = Convert.ToInt32(numThreshold.Value);
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            CLICKDELAY = Convert.ToInt32(numDelay.Value);
        }

        private void cbMouseClick_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbMouseClick.Checked)
            {
                mouseClickModeOn = false;
            }
            else
            {
                mouseClickModeOn = true;
            }
        }

        private void numSmoothSize_ValueChanged(object sender, EventArgs e)
        {
            WINDOWSIZE = Convert.ToInt32(numSmoothSize.Value);
        }

    }
}
