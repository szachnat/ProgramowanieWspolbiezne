using System;

namespace TPW.Dane
{
    public delegate void PositionChangeEventHandler(object source, PositionChangeEventArgs e);

    public class PositionChangeEventArgs : EventArgs
    {
        public Pos2D LastPos { get; private set; }
        public Pos2D Vel { get; private set; }
        public double ElapsedSeconds { get; private set; }

        /// <summary>
        /// Wywoływane gdy zmiana pozycji
        /// </summary>
        /// <param name="lastPos">Poprzednia pozycja</param>
        /// <param name="newPos">Nowa pozytcja</param>
        /// <param name="seconds">Czas pomiędzy pozycjami</param>
        public PositionChangeEventArgs(Pos2D lastPos, Pos2D vel, double seconds)
        {
            LastPos = lastPos;
            Vel = vel;
            ElapsedSeconds = seconds;
        }
    }
}
