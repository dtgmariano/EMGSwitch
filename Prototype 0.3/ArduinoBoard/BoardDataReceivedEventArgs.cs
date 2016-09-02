using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoBoard
{
    public class BoardDataReceivedEventArgs: EventArgs
    {
        public BoardDataReceivedEventArgs(long epoch, UInt16[] chA_Data, int overflow)
        {
            Epoch = epoch;
            ChA_Data = chA_Data;
            Overflow = overflow;
        }

        public long Epoch { get; protected set; }

        public UInt16[] ChA_Data { get; protected set; }

        public int Overflow { get; protected set; }
    }
}
