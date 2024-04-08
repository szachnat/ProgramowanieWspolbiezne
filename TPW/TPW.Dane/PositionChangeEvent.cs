using System;

namespace TPW.Dane
{
    public delegate void PositionChangeEventHandler(object source, PositionChangeEventArgs e);

    public class PositionChangeEventArgs : EventArgs
    {
        public Pos2D LastPos { get; private set; }
        public Pos2D NewPos { get; private set; }
        public double ElapsedSeconds { get; private set; }

        public PositionChangeEventArgs(Pos2D lastPos, Pos2D newPos, double seconds)
        {
            LastPos = lastPos;
            NewPos = newPos;
            ElapsedSeconds = seconds;
        }
    }
}
