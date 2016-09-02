using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArduinoBoard;

namespace DataPlotter
{
    public partial class Form1 : Form
    {
        Board board;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void ControllerOnDataReceived(object sender, BoardDataReceivedEventArgs boardDataReceivedEventArgs)
        {
            var dt = boardDataReceivedEventArgs.ChA_Data;
            var ep = boardDataReceivedEventArgs.Epoch;

            Console.WriteLine("Epoch: " + ep);
            foreach(ushort u in dt)
            {
                Console.Write(dt + "\t");
            }
            Console.WriteLine();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            board = new Board("COM4");
            board.DataReceived += ControllerOnDataReceived;
            board.Open();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            board.DataReceived -= ControllerOnDataReceived;
            board.Close();
            this.Dispose();
        }
    }
}
