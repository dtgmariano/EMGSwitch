using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ArduinoBoard
{
    public class Board
    {
        public const int limit_buffer = 50;
        public const int baudRate = 19200;

        private readonly byte[] _buffer = new byte[limit_buffer];
        private readonly object _lock = new object();
        private readonly SerialPort _serialPort;
        //private long _epoch;

        public const int btr = (6 + 2 * 2);

        public Board(string portName)
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        }

        public bool IsOpen
        {
            get { return _serialPort.IsOpen; }
        }

        public event EventHandler<BoardDataReceivedEventArgs> DataReceived;

        protected void OnDataReceived(BoardDataReceivedEventArgs args)
        {
            if (DataReceived != null)
            {
                DataReceived(this, args);
            }
        }

        public void Open()
        {
            if (!IsOpen)
            {
                _serialPort.Open();
                Sync();
            }
        }

        public void Close()
        {
            _serialPort.Close();
        }

        private void Sync()
        {
            _serialPort.DataReceived -= sp_DataReceived;
            _serialPort.DiscardInBuffer();
            //_epoch = 0;

            var buf = new byte[limit_buffer];
            var btr = _serialPort.BytesToRead;
            if (_serialPort.Read(buf, 0, btr) == limit_buffer)
            {
                Console.WriteLine("Synchronized");
            }

            _serialPort.ReceivedBytesThreshold = limit_buffer;
            _serialPort.DataReceived += sp_DataReceived;
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock(_lock)
            {
                var sp = (SerialPort)sender;

                while(sp.BytesToRead > btr)
                {
                    /*Verifica se o o primeiro byte do array é o header*/
                    bool test = ((_buffer[0] == 51) && (_buffer[1] == 204)); 

                    if(test)
                    {
                        int epoch = _buffer[2];
                        int sizeChA = _buffer[3];
                        int ovf = _buffer[4];
                        int sizeHeader = 5;

                        UInt16[] var = new UInt16[sizeChA];

                        for (int i = 0; i < sizeChA; i++)
                            var[i] = Rebuilder(_buffer[(i * 2) + sizeHeader], _buffer[(i * 2) + sizeHeader]);

                        OnDataReceived(new BoardDataReceivedEventArgs(epoch, var, ovf));
                        //OnDataReceived(new BoardDataReceivedEventArgs(Interlocked.Increment(ref _epoch), var, ovf));
                    }
                }
            }
        }

        private UInt16 Rebuilder(byte msb, byte lsb)
        {
            ushort h = (ushort)(msb & 31);
            h = (ushort)(h << 5);
            ushort l = lsb;
            return (UInt16)(h | l);
        }

    }
}
