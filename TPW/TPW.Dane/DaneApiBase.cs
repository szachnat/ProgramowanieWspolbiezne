namespace TPW.Dane
{
    public abstract class DaneApiBase
    {
        public abstract Ball CreateRandomBall(double radius, Pos2D minPos, Pos2D maxPos, double minVel, double maxVel);
        public abstract Plane CreatePlane(double width, double height);

        public static DaneApiBase GetApi()
        {
            return new DaneApi();
        }
    }
}
