using System;

namespace TPW.Dane
{
    /// <summary>
    /// Interfejs implementowany prze kulkę
    /// </summary>
    public interface IBall : INotifyPositionChanged, IDisposable
    {
        #region BallBase
        void SetMass(double mass);
        void SetRadius(double radius);
        void SetPos(Pos2D pos);
        void SetVel(Pos2D vel);
        long GetId();
        double GetMass();
        double GetRadius();
        Pos2D GetPos();
        Pos2D GetVel();

        #endregion BallBase

        #region Thread
        void StartThread();
        void EndThread();

        #endregion Thread
    }
}
