using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ArduinoBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            var ports = SerialPort.GetPortNames(); 
            Board board = new Board("COM4");
            board.Open();

            Console.WriteLine("");

            while(true)
            {
                
            }
        }
    }
}
