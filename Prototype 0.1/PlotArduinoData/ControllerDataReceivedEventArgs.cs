namespace Board
{
    using System;

    public class ControllerDataReceivedEventArgs: EventArgs
    {
        public ControllerDataReceivedEventArgs(long epoch, UInt16[] boardData)
        {
            Epoch = epoch;
            BoardData = boardData;
        }

        public long Epoch { get; protected set; }

        public UInt16[] BoardData { get; protected set; }
    }
}
