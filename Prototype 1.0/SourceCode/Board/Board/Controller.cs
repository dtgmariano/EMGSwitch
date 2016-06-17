namespace Board
{
    using System;
    using System.IO.Ports;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Controller
    {
        public const int maxSizeBuffer = 50;
        private readonly byte[] _buffer = new byte[maxSizeBuffer];
        private readonly object _lock = new object();
        private readonly SerialPort _serialPort;

        private long _epoch;
        public const int btr = (5 + 2 * 10);

        public Controller(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        }
        public bool IsOpen
        {
            get { return _serialPort.IsOpen; }
        }

        public event EventHandler<ControllerDataReceivedEventArgs> DataReceived;
        //public event EventHandler ControllerSynchronized;

        protected void OnDataReceived(ControllerDataReceivedEventArgs args)
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
            _epoch = 0;

            var buf = new byte[maxSizeBuffer];
            var btr = _serialPort.BytesToRead;


            if (_serialPort.Read(buf, 0, btr) == maxSizeBuffer)
            {
                //Console.WriteLine("Synchronized");
            }

            _serialPort.ReceivedBytesThreshold = maxSizeBuffer;
            _serialPort.DataReceived += sp_DataReceived;
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_lock)
            {
                var sp = (SerialPort)sender;

                while (sp.BytesToRead > btr)
                {
                    

                    sp.Read(_buffer, 0, _buffer.Length);

                    bool test = (_buffer[0] == 51 && _buffer[1] == 204);

                    if (test)
                    {
                        int package = _buffer[2];
                        int nPoints = _buffer[3];
                        UInt16[] var = new UInt16[nPoints];
                        int headerSize = 4;

                        for (int i = 0; i < nPoints; i++)
                        {
                            ushort hb = (ushort)(_buffer[(i * 2) + headerSize] & 31);
                            hb = (ushort)(hb << 5);
                            ushort lb = _buffer[(i * 2 + 1) + headerSize];
                            var[i] = (UInt16)(hb | lb);
                        }

                        OnDataReceived(new ControllerDataReceivedEventArgs(Interlocked.Increment(ref _epoch), var));
                    }
                    else
                    {
                        Sync();
                    }
                }
            }
        }
    }
}
