namespace PlotArduinoData
{
    partial class PlotDataView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.bStart = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbStatus = new System.Windows.Forms.Label();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerSerial = new System.Windows.Forms.Timer(this.components);
            this.timerClick = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbMouseClick = new System.Windows.Forms.CheckBox();
            this.cbGraph = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numGraphSize = new System.Windows.Forms.NumericUpDown();
            this.lbPort = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numWindowSize = new System.Windows.Forms.NumericUpDown();
            this.lbSF = new System.Windows.Forms.Label();
            this.numSF = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPortName = new System.Windows.Forms.ComboBox();
            this.lbSt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numSmoothSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGraphSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWindowSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmoothSize)).BeginInit();
            this.SuspendLayout();
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(5, 352);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(41, 23);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "Start";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(47, 352);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(41, 23);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // chart
            // 
            chartArea2.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(3, 6);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(987, 479);
            this.chart.TabIndex = 2;
            this.chart.Text = "chart1";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(339, 544);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(0, 13);
            this.lbStatus.TabIndex = 3;
            // 
            // numThreshold
            // 
            this.numThreshold.Location = new System.Drawing.Point(6, 65);
            this.numThreshold.Maximum = new decimal(new int[] {
            1050,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(83, 20);
            this.numThreshold.TabIndex = 4;
            this.numThreshold.Value = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.numThreshold.ValueChanged += new System.EventHandler(this.numThreshold_ValueChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 68);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(251, 267);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 56);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // timerSerial
            // 
            this.timerSerial.Tick += new System.EventHandler(this.timerSerial_Tick);
            // 
            // timerClick
            // 
            this.timerClick.Tick += new System.EventHandler(this.timerClick_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart);
            this.panel1.Location = new System.Drawing.Point(117, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(993, 496);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(1116, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(257, 340);
            this.panel2.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.numSmoothSize);
            this.panel3.Controls.Add(this.cbMouseClick);
            this.panel3.Controls.Add(this.cbGraph);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.numGraphSize);
            this.panel3.Controls.Add(this.lbPort);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.numWindowSize);
            this.panel3.Controls.Add(this.lbSF);
            this.panel3.Controls.Add(this.numSF);
            this.panel3.Controls.Add(this.bStop);
            this.panel3.Controls.Add(this.bStart);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.numDelay);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.cbPortName);
            this.panel3.Controls.Add(this.numThreshold);
            this.panel3.Location = new System.Drawing.Point(12, 77);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(99, 417);
            this.panel3.TabIndex = 9;
            // 
            // cbMouseClick
            // 
            this.cbMouseClick.AutoSize = true;
            this.cbMouseClick.Checked = true;
            this.cbMouseClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMouseClick.Location = new System.Drawing.Point(4, 310);
            this.cbMouseClick.Name = "cbMouseClick";
            this.cbMouseClick.Size = new System.Drawing.Size(49, 17);
            this.cbMouseClick.TabIndex = 17;
            this.cbMouseClick.Text = "Click";
            this.cbMouseClick.UseVisualStyleBackColor = true;
            this.cbMouseClick.CheckedChanged += new System.EventHandler(this.cbMouseClick_CheckedChanged);
            // 
            // cbGraph
            // 
            this.cbGraph.AutoSize = true;
            this.cbGraph.Checked = true;
            this.cbGraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGraph.Location = new System.Drawing.Point(4, 287);
            this.cbGraph.Name = "cbGraph";
            this.cbGraph.Size = new System.Drawing.Size(55, 17);
            this.cbGraph.TabIndex = 16;
            this.cbGraph.Text = "Graph";
            this.cbGraph.UseVisualStyleBackColor = true;
            this.cbGraph.CheckedChanged += new System.EventHandler(this.cbGraph_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Tamanho Graph";
            // 
            // numGraphSize
            // 
            this.numGraphSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numGraphSize.Location = new System.Drawing.Point(5, 261);
            this.numGraphSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numGraphSize.Name = "numGraphSize";
            this.numGraphSize.Size = new System.Drawing.Size(83, 20);
            this.numGraphSize.TabIndex = 14;
            this.numGraphSize.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lbPort
            // 
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(6, 6);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(32, 13);
            this.lbPort.TabIndex = 13;
            this.lbPort.Text = "Porta";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Tamanho Janela";
            // 
            // numWindowSize
            // 
            this.numWindowSize.Location = new System.Drawing.Point(5, 222);
            this.numWindowSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWindowSize.Name = "numWindowSize";
            this.numWindowSize.Size = new System.Drawing.Size(83, 20);
            this.numWindowSize.TabIndex = 11;
            this.numWindowSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lbSF
            // 
            this.lbSF.AutoSize = true;
            this.lbSF.Location = new System.Drawing.Point(3, 127);
            this.lbSF.Name = "lbSF";
            this.lbSF.Size = new System.Drawing.Size(60, 13);
            this.lbSF.TabIndex = 10;
            this.lbSF.Text = "Frequência";
            // 
            // numSF
            // 
            this.numSF.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSF.Location = new System.Drawing.Point(6, 143);
            this.numSF.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSF.Name = "numSF";
            this.numSF.Size = new System.Drawing.Size(83, 20);
            this.numSF.TabIndex = 9;
            this.numSF.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Delay";
            // 
            // numDelay
            // 
            this.numDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numDelay.Location = new System.Drawing.Point(6, 104);
            this.numDelay.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(83, 20);
            this.numDelay.TabIndex = 7;
            this.numDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Limiar";
            // 
            // cbPortName
            // 
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Location = new System.Drawing.Point(6, 25);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new System.Drawing.Size(83, 21);
            this.cbPortName.TabIndex = 5;
            this.cbPortName.SelectedIndexChanged += new System.EventHandler(this.cbPortName_SelectedIndexChanged);
            this.cbPortName.Click += new System.EventHandler(this.cbPortName_Click);
            // 
            // lbSt
            // 
            this.lbSt.AutoSize = true;
            this.lbSt.Location = new System.Drawing.Point(14, 9);
            this.lbSt.Name = "lbSt";
            this.lbSt.Size = new System.Drawing.Size(0, 13);
            this.lbSt.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Smooth Size";
            // 
            // numSmoothSize
            // 
            this.numSmoothSize.Location = new System.Drawing.Point(6, 182);
            this.numSmoothSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSmoothSize.Name = "numSmoothSize";
            this.numSmoothSize.Size = new System.Drawing.Size(83, 20);
            this.numSmoothSize.TabIndex = 18;
            this.numSmoothSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSmoothSize.ValueChanged += new System.EventHandler(this.numSmoothSize_ValueChanged);
            // 
            // PlotDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 517);
            this.Controls.Add(this.lbSt);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbStatus);
            this.Name = "PlotDataView";
            this.Text = "EMG-MouseClick";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGraphSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWindowSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmoothSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerSerial;
        private System.Windows.Forms.Timer timerClick;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbPortName;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numWindowSize;
        private System.Windows.Forms.Label lbSF;
        private System.Windows.Forms.NumericUpDown numSF;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numGraphSize;
        private System.Windows.Forms.Label lbSt;
        private System.Windows.Forms.CheckBox cbGraph;
        private System.Windows.Forms.CheckBox cbMouseClick;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSmoothSize;
    }
}

