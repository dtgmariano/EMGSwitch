namespace Board
{
    
    using System;
    using System.IO.Ports;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Controller
    {
        public const int serialBytesSize = 24;
        private readonly byte[] _buffer = new byte[serialBytesSize];
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

            var buf = new byte[serialBytesSize];
            if (_serialPort.Read(buf, 0, buf.Length) == serialBytesSize)
            {
             
                //Console.WriteLine("Synchronized");
            }

            _serialPort.ReceivedBytesThreshold = serialBytesSize;
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
                    int varSize = 10;
                    UInt16[] var = new UInt16[varSize];
                    int headerSize = 4;
                    bool test = (_buffer[0] == 204) && (_buffer[1] == 170);
                    if (test)
                    {
                        //package
                        ushort hbpck = (ushort)(_buffer[2] & 31);
                        hbpck = (ushort)(hbpck << 5);
                        ushort lbpck = _buffer[3];
                        _epoch = (UInt16)(hbpck | lbpck);

                        //signal data
                        for (int i = 0; i < varSize; i++)
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

                    }
                    #region old
                    //if (_buffer[0]>=224 && _buffer[0]<=255)
                    //{
                    //    ushort hb = (ushort)(_buffer[0] & 31);
                    //    hb = (ushort)(hb << 5);
                    //    ushort lb = _buffer[1];
                    //    var[0] = (UInt16)(hb | lb);
                    //    OnDataReceived(new ControllerDataReceivedEventArgs(Interlocked.Increment(ref _epoch), var));
                    //}
                    //else
                    //{
                    //    Sync();
                    //}
                    #endregion old
                }
            }
        }
    }
}
