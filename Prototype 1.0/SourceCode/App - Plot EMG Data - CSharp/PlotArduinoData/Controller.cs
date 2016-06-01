namespace Board
{
    using System;
    using System.IO.Ports;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Controller
    {
        private readonly byte[] _buffer = new byte[2];
        private readonly object _lock = new object();
        private readonly SerialPort _serialPort;
        private long _epoch;

        public Controller(string portName)
        {
            _serialPort = new SerialPort(portName, 19200, Parity.None, 8, StopBits.One);
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

            var buf = new byte[2];
            if (_serialPort.Read(buf, 0, buf.Length) == 2)
            {
                //Console.WriteLine("Synchronized");
            }

            _serialPort.ReceivedBytesThreshold = 2;
            _serialPort.DataReceived += sp_DataReceived;
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_lock)
            {
                var sp = (SerialPort) sender;
                while (sp.BytesToRead > 1)
                {
                    sp.Read(_buffer, 0, _buffer.Length);
                    UInt16[] var = new UInt16[1];
                    
                    if(_buffer[0]>=224 && _buffer[0]<=255)
                    {
                        ushort hb = (ushort)(_buffer[0] & 31);
                        hb = (ushort)(hb << 5);
                        ushort lb = _buffer[1];
                        var[0] = (UInt16)(hb | lb);
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
